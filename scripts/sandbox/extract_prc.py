from pathlib import Path
import sys
import pikepdf

if len(sys.argv) != 3:
    print("Usage: py extract_prc.py input.pdf output.prc")
    sys.exit(1)

input_pdf = sys.argv[1]
output_prc = sys.argv[2]

with pikepdf.open(input_pdf) as pdf:
    found = False

    for page_number, page in enumerate(pdf.pages, start=1):
        annotations = page.get("/Annots", [])

        for annotation_ref in annotations:
            annotation = annotation_ref.get_object()

            if annotation.get("/Subtype") != "/3D":
                continue

            print(f"Found 3D annotation on page {page_number}")

            model_ref = annotation.get("/3DD")

            if model_ref is None:
                print("The annotation has no /3DD object")
                continue

            model = model_ref.get_object()
            model_type = str(model.get("/Subtype", ""))

            print(f"Embedded model type: {model_type}")

            if model_type != "/PRC":
                continue

            data = model.read_bytes()
            Path(output_prc).write_bytes(data)

            print(f"Extracted {len(data)} bytes")
            print(f"Saved to: {output_prc}")

            found = True
            break

        if found:
            break

    if not found:
        print("No PRC 3D stream was found")
        sys.exit(2)

### 2026-08-02
--------------

**The benefits of Razor Pages**

Razor syntax combines HTML and C# to define the dynamic rendering logic.
Razor Pages is a server-side, page-centric programming model for building web UIs with ASP.NET Core. Benefits include:
* Easy setup for dynamic web apps using HTML, CSS, and C#.
* Organized files by feature for easier maintenance.
* Combines markup with server-side C# code using Razor syntax.

`Razor Pages` utilize `Razor` for embedding server-based code into webpages.

Razor Pages обеспечивает разделение ответственности с помощью класса PageModel на C#, инкапсулирующего свойства данных и логические операции, относящиеся к конкретной странице Razor, а также определяющего обработчики страниц для HTTP-запросов.

Use Razor Pages to keep ASP.NET Core pages organized in a simpler way. All view (page) specific logic and page properties (view model) are kept in the same proximity.

```powershell
git clone https://github.com/MicrosoftDocs/mslearn-create-razor-pages-aspnet-core
mv "mslearn-create-razor-pages-aspnet-core" "mslearn-razor-aspnet-core"

```

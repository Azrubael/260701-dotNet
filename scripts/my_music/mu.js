function noteNum(n) {
  const m = /^([A-G])(#|b)?(\d)$/.exec(n);
  const base = { C: 0, D: 2, E: 4, F: 5, G: 7, A: 9, B: 11 }[m[1]];
  return base + (m[2] === '#' ? 1 : m[2] === 'b' ? -1 : 0) + (+m[3] + 1) * 12;
}
const mtof = m => 440 * Math.pow(2, (m - 69) / 12);
function parseMelody(bars) {
  const ev = {}; let last = null, i = 0;
  for (const bar of bars) for (const tok of bar.trim().split(/\s+/)) {
    if (tok === '-') { if (last) last.len++; }
    else if (tok === '.') last = null;
    else { last = { n: noteNum(tok), len: 1 }; ev[i] = last; }
    i++;
  }
  return ev;
}
const TRACKS = {
  title: {
    bpm: 118, bars: 4, loop: true, roots: [45, 41, 43, 40], quals: 'mMMM',
    bass: [0, null, null, 12, null, null, 0, null, 0, null, null, 12, null, 7, null, null],
    lead: parseMelody([
      'A4 - C5 - E5 - A5 - G5 - E5 - D5 - E5 -',
      'C5 - A4 - F4 - A4 - C5 - F5 - E5 - C5 -',
      'D5 - - - B4 - G4 - B4 - D5 - G5 - - -',
      'G#5 - - - E5 - B4 - G#4 - B4 - E5 - - -']),
    drums: { k: 'x.......x.......', s: '....x.......x...', h: '..x...x...x...x.' }, arp: 0.03
  },
  stage: {
    bpm: 152, bars: 8, loop: true, roots: [45, 45, 41, 43, 45, 48, 43, 40], quals: 'mmMMmMMM',
    bass: [0, null, 0, 12, 0, null, 0, 12, 0, null, 0, 12, 0, 7, 10, 12],
    lead: parseMelody([
      'A4 - C5 - E5 - A5 - G5 - E5 - D5 - E5 -',
      'C5 - - - B4 - A4 - B4 - C5 - D5 - - -',
      'C5 - A4 - F4 - A4 - C5 - F5 - E5 - C5 -',
      'D5 - - - B4 - G4 - B4 - D5 - G5 - - -',
      'A5 - - - G5 - E5 - C5 - E5 - A5 - C6 -',
      'G5 - - - E5 - C5 - E5 - G5 - C6 - B5 -',
      'B5 - A5 - G5 - D5 - B4 - D5 - G5 - A5 -',
      'G#5 - - - E5 - B4 - G#4 - B4 - E5 - - -']),
    drums: { k: 'x...x...x...x.x.', s: '....x.......x..x', h: 'x.x.x.x.x.x.x.xx' }, arp: 0.028
  },
  boss: {
    bpm: 172, bars: 8, loop: true, roots: [40, 40, 41, 40, 40, 41, 43, 41], quals: 'mmMmmMMM',
    bass: [0, 0, 12, 0, 0, 0, 12, 0, 0, 0, 12, 0, 1, 0, 12, 1],
    lead: parseMelody([
      'E5 . E5 . F5 . E5 . B4 - - - . . . .',
      'E5 . E5 . G5 . F5 . E5 - D5 - C5 - B4 -',
      'F5 . F5 . G#5 . F5 . C5 - - - . . . .',
      'E5 . E5 . B5 . A5 . G5 - F5 - E5 - F5 -',
      'E6 - B5 - G5 - E5 - E6 - B5 - G5 - E5 -',
      'F6 - C6 - A5 - F5 - F6 - C6 - A5 - F5 -',
      'G6 - D6 - B5 - G5 - G6 - D6 - B5 - G5 -',
      'F6 - E6 - D6 - C6 - B5 - A5 - G#5 - B5 -']),
    drums: { k: 'x...x...x...x...', s: '....x.......x.xx', h: 'xxxxxxxxxxxxxxxx', k2: '..x.......x.....' }, arp: 0.03
  },
  victory: {
    bpm: 140, bars: 3, loop: false, roots: [48, 43, 48], quals: 'MMM',
    bass: [0, null, null, null, 0, null, null, null, 7, null, null, null, 12, null, null, null],
    lead: parseMelody(['G4 . C5 . E5 . G5 - - . E5 . G5 - - -', 'F5 . A5 . G5 - F5 - D5 - B4 - G4 - - -', 'C5 . E5 . G5 . C6 - - - - - - - . .']),
    drums: { k: 'x.......x.......', s: '....x.......x...', h: 'x.x.x.x.x.x.x.x.' }, arp: 0
  },
  gameover: {
    bpm: 90, bars: 2, loop: false, roots: [45, 40], quals: 'mM',
    bass: [0, null, null, null, null, null, null, null, 0, null, null, null, null, null, null, null],
    lead: parseMelody(['E5 - D5 - C5 - B4 - A4 - - - G#4 - - -', 'A4 - - - - - - - . . . . . . . .']),
    drums: { k: 'x...............', s: '................', h: '................' }, arp: 0
  },
  jingle: {
    bpm: 160, bars: 1, loop: false, roots: [48], quals: 'M',
    bass: [0, null, null, null, 7, null, null, null, 12, null, null, null, null, null, null, null],
    lead: parseMelody(['C5 . E5 . G5 . C6 - - - G5 . C6 - - -']),
    drums: { k: 'x.......x.......', s: '............x...', h: '..x...x...x.....' }, arp: 0
  }
};
const Music = {
  cur: null, tr: null, step: 0, next: 0, timer: null, pending: null,
  play(name) {
    if (!Snd.ctx) { this.pending = name; return; }
    if (this.cur === name) return;
    this.stop();
    this.cur = name; this.tr = TRACKS[name]; this.step = 0;
    this.next = Snd.ctx.currentTime + 0.08;
    this.timer = setInterval(() => this.tick(), 25);
    this.tick();
  },
  stop() { if (this.timer) clearInterval(this.timer); this.timer = null; this.cur = null; this.pending = null; },
  duck(on) { if (Snd.mus) Snd.mus.gain.setTargetAtTime(on ? 0.08 : 0.32, Snd.ctx.currentTime, 0.05); },
  tick() {
    const c = Snd.ctx, tr = this.tr;
    if (!c || !tr) return;
    const spb = 60 / tr.bpm / 4, total = tr.bars * 16;
    if (this.next < c.currentTime - 0.3) this.next = c.currentTime + 0.05; // tab was suspended
    while (this.next < c.currentTime + 0.15) {
      if (!tr.loop && this.step >= total) { this.stop(); return; }
      this.sched(this.step % total, this.next, spb);
      this.next += spb; this.step++;
    }
  },
  sched(i, t, spb) {
    if (Snd.muted) return;
    const tr = this.tr, bar = (i / 16) | 0, s = i % 16, root = tr.roots[bar], q = tr.quals[bar];
    const d = tr.drums;
    if (d.k[s] === 'x' || (d.k2 && d.k2[s] === 'x')) this.kick(t);
    if (d.s[s] === 'x') this.snare(t);
    if (d.h[s] === 'x') this.hat(t, s % 4 === 2 ? 0.06 : 0.035);
    const b = tr.bass[s];
    if (b !== null && b !== undefined) this.inst('sawtooth', mtof(root - 12 + b), t, spb * 0.9, 0.17, 700);
    if (tr.arp) {
      const ch = [0, q === 'm' ? 3 : 4, 7, 12][s % 4];
      this.inst('triangle', mtof(root + 24 + ch), t, spb * 0.7, tr.arp);
    }
    const n = tr.lead[i];
    if (n) {
      this.inst('pulse', mtof(n.n), t, spb * n.len * 0.92, 0.075);
      this.inst('square', mtof(n.n) * 1.005, t, spb * n.len * 0.8, 0.02);
    }
  },
  inst(type, f, t, dur, vol, lp) {
    const c = Snd.ctx, o = c.createOscillator();
    if (type === 'pulse') o.setPeriodicWave(Snd.pulse); else o.type = type;
    o.frequency.setValueAtTime(f, t);
    const g = c.createGain();
    g.gain.setValueAtTime(0.0001, t);
    g.gain.exponentialRampToValueAtTime(vol, t + 0.006);
    g.gain.setValueAtTime(vol, t + Math.max(0.01, dur - 0.03));
    g.gain.exponentialRampToValueAtTime(0.0001, t + dur + 0.04);
    if (lp) {
      const f2 = c.createBiquadFilter(); f2.type = 'lowpass'; f2.frequency.value = lp; f2.Q.value = 3;
      o.connect(f2); f2.connect(g);
    } else o.connect(g);
    g.connect(Snd.mus); o.start(t); o.stop(t + dur + 0.06);
  },
  kick(t) {
    const c = Snd.ctx, o = c.createOscillator(), g = c.createGain();
    o.frequency.setValueAtTime(150, t); o.frequency.exponentialRampToValueAtTime(38, t + 0.13);
    g.gain.setValueAtTime(0.9, t); g.gain.exponentialRampToValueAtTime(0.0001, t + 0.22);
    o.connect(g); g.connect(Snd.mus); o.start(t); o.stop(t + 0.25);
  },
  snare(t) {
    const c = Snd.ctx, s = c.createBufferSource(); s.buffer = Snd.noiseBuf;
    const f = c.createBiquadFilter(); f.type = 'highpass'; f.frequency.value = 1100;
    const g = c.createGain(); g.gain.setValueAtTime(0.45, t); g.gain.exponentialRampToValueAtTime(0.0001, t + 0.16);
    s.connect(f); f.connect(g); g.connect(Snd.mus); s.start(t, Math.random() * 0.5); s.stop(t + 0.18);
    const o = c.createOscillator(), g2 = c.createGain(); o.type = 'triangle';
    o.frequency.setValueAtTime(190, t); o.frequency.exponentialRampToValueAtTime(120, t + 0.08);
    g2.gain.setValueAtTime(0.3, t); g2.gain.exponentialRampToValueAtTime(0.0001, t + 0.1);
    o.connect(g2); g2.connect(Snd.mus); o.start(t); o.stop(t + 0.12);
  },
  hat(t, v) {
    const c = Snd.ctx, s = c.createBufferSource(); s.buffer = Snd.noiseBuf;
    const f = c.createBiquadFilter(); f.type = 'highpass'; f.frequency.value = 7000;
    const g = c.createGain(); g.gain.setValueAtTime(v, t); g.gain.exponentialRampToValueAtTime(0.0001, t + 0.045);
    s.connect(f); f.connect(g); g.connect(Snd.mus); s.start(t, Math.random() * 0.5); s.stop(t + 0.06);
  }
};
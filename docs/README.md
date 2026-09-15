# PillowNet documentation

**Start here:** [index.html](./index.html) — project overview, architecture, and quick start.

## HTML docs

| Document | Description |
|----------|-------------|
| [API Reference](./api.html) | C# API, CSnakes bridge internals, CLI reference |
| [Feature Plan](./features.html) | Pillow module coverage and roadmap |

## Source layout

| Path | Purpose |
|------|---------|
| `index.html` | Project landing page |
| `pages/*.md` | Markdown sources for topic HTML pages |
| `theme/` | MDWeb layout template |
| `assets/css/portfolio.css` | Styles for `index.html` |
| `assets/css/site.css` | Styles for MDWeb-generated topic pages |
| `build-docs.sh` | Regenerate topic HTML with MDWeb |

```bash
./docs/build-docs.sh
```

Requires [MDWeb](https://github.com/hoihky/MDWeb) cloned as a sibling repository (`../MDWeb`), or set `MDWEB_CLI` to your `MDWeb.Cli` project path.

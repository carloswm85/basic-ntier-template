![logo](./docs/img/banner.png)

# Basic N-Tier Template

> <https://github.com/carloswm85/basic-ntier-template>

| #     | Template                  | Documentation                                                         | Layering   | Structured By   | Status            | Available / Ready to use | .NET Core Versions |
| ----- | ------------------------- | --------------------------------------------------------------------- | ---------- | --------------- | ----------------- | ------------------------ | ------------------ |
| `NTA` | _Basic-Ntier-Template_    | [README.md](./templates/BasicNtierTemplate/docs/onboarding/README.md) | Horizontal | Function(ality) | Under Development | 🟢 YES                   | 8, 10              |
| `VSA` | _Vertical-Slice-Template_ | -                                                                     | Vertical   | Features        | Planned           | 🔴 NO                    | -                  |

Overview:

- `NTA` - **N-Tier Architecture**: It prioritizes separation of concerns and infrastructure layering. It guides developers to think in terms of horizontal tiers (presentation, business logic, data) — not end-to-end features. While it introduces indirection, latency, and scattered changes, the benefits in security, independent scaling, and traditional maintainability make it a proven choice for enterprise systems.
- `VSA` - **Vertical Slice Architecture**: It favors delivering value over internal purity. It guides developers to think in terms of features, workflows, and outcomes — not infrastructure. While it introduces some duplication and requires mindset shifts, the improvements in autonomy, clarity, and scalability make it a compelling architecture for modern modular systems.

## Further Developments

- Clean Architecture.

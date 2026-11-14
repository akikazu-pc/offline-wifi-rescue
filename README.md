# 断网急救 · Offline Wi-Fi Rescue

面向 Windows 10 / Windows 11 的开源 Wi-Fi 驱动急救工具。目标是在重装系统后，用 U 盘恢复无线联网。

**状态：仓库包含可独立构建的 Windows 桌面应用、WinPE 工具和驱动匹配引擎。发布包由本地构建脚本生成。**

| 版本 | 目标设备 | 选包依据 |
| --- | --- | --- |
| 台式机 | AM4、AM5、LGA1700、LGA1851 组装机，兼顾后装 PCIe 无线网卡 | 硬件 ID、子系统 ID、系统版本和架构，结合主板型号及修订版 |
| 笔记本 | 各品牌 Windows 笔记本，首发目标为 Windows 10/11 x64 | 硬件 ID、子系统 ID、系统版本和架构，结合 OEM 机型及扩展 INF |

同一个「断网急救」安装包服务台式机和笔记本。在线安装版可按需下载驱动。离线完整版免安装，包含通用 WLAN 驱动与 OEM 补充包。两类设备分别维护覆盖证据。第一阶段以 AM4、AM5、LGA1700、LGA1851 全品牌主板逐板核对为验收目标，覆盖公开可查的区域型号与硬件修订。台式机覆盖验收前，有出厂 Wi-Fi 的主板必须有可追溯的网卡及驱动对应关系。

详见[第一阶段覆盖验收与来源调查](docs/phase-one-coverage.md)和[调研与首版设计](docs/research.md)。

代码采用 MIT 许可。第三方驱动各自适用其发布方的许可条款。

恢复联网后，可进入下一阶段规划的[按需下载和自动安装驱动](docs/online-driver-install.md)。

参见[安装版本、驱动升级与配置工具](docs/distribution-and-updates.md)。

## 开发顺序

1. 建立可冻结的主板目录、证据格式和缺口报表。同时实现只读设备扫描和导出，采集实际硬件 ID。
2. 构建 INF 索引、离线匹配与安装验证。通过四平台的覆盖验收后验收台式机覆盖并发布统一离线免安装版。笔记本的 OEM 资料和实机验证另行推进。
3. 基于同一引擎交付在线安装版的联网补装，再验证驱动升级、备份和回退。
4. 在 Windows 上提前验证 Store 配置工具的最小联动原型，完成双装、单托盘和上架验证后发布配置工具。

[断网急救与硬件测试的独立安装和联动](docs/shared-desktop-apps.md)

## 本地构建与验证

在 Windows x64 上安装 .NET 10 SDK 和 PowerShell 7。WinPE 编译另需 Visual Studio Build Tools 的 MSVC x64 和 Windows SDK。

```powershell
dotnet build src/AkikazuHardwareCheck.App -c Release
dotnet run --project tests/AkikazuHardwareCheck.Core.Tests -c Release
cmd /c scripts\build-network-rescue-pe.cmd
pwsh -NoProfile -File scripts/publish-network-rescue-bundles.ps1
```

发布脚本默认生成在线安装 ZIP 和 SHA-256 文件，输出到 artifacts/network-rescue。传入 -RealtekVendorZip 可额外构建离线预览 ZIP。公开离线 ESD 使用 -PublicOffline、原厂驱动输入和管理员 PowerShell 中的 DISM。附加驱动的输入文件名和校验值见 [公开驱动清单](packaging/shared-desktop/public-offline-drivers.json)。

本次迁移基于 hardware-check 的 cb90eda。公开离线打包与驱动清单另取自 fdd8df6。驱动 ZIP 由操作者提供。
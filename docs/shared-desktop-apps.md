# 两款桌面应用的安装与联动

硬件测试使用现有 hardware-check 桌面项目。断网急救和硬件测试各有独立安装标识、启动入口和卸载入口。可只装任一应用，单装时即可使用该应用的核心功能和自身设置。断网急救提供在线安装版和离线完整版。在线版有独立安装身份，离线版解包即可运行。

双装时，两边入口都可打开共用主窗口，并分别定位到硬件测试或驱动急救。主窗口与可选托盘使用每用户会话单实例。共享组件通过稳定的版本化接口发现已安装模块。两个安装器都可提供所需主程序组件，独立升级和卸载时保留另一款仍需使用的兼容组件。卸载后重新打开剩余应用，核心功能、设置和启动入口应仍可使用。单装时可显示另一应用的官方获取入口。

保留 hardware-check 现有官网与商店分发能力边界。官网安装版的合并窗口先行验证。商店渠道与断网急救的配置入口联动另做原型，商店配置入口不作为任一应用的运行依赖。驱动安装单独提权，硬件测试沿用自身权限策略。

首轮开发只实现共享窗口与模块发现的最小原型，验证只装硬件测试、只装断网急救、两种双装顺序、同时启动、各自升级及卸载任一应用。双装启用托盘时只显示一个图标。断网时也能启动两个本地模块。覆盖数据库、真实驱动安装与商店发布留给后续验收。

## 独立版的交付与启动

- 在线安装 ZIP 解包后运行 `Install.cmd`。安装器使用共享注册契约下的 `Akikazu.NetworkRescue` 标识，安装独立的 `NetworkRescueDesktop.exe`。
- 公开离线 ESD 包含 Realtek 8852、8851 和 8922 驱动。8851 有 Win10 和 Win11 版本，8922 面向 Win11。公开打包仅纳入清单中的原厂文件和版权声明。 [发行清单](release-checklist.md)
- 本地离线完整版也是 ESD。构建时用 `-IncludeCatalog -WlanArchive` 指定固定的 SDIO WLAN 归档，另外传入 `-PeEthernetPackDirectory` 和 `-SevenZipExecutable`。有线驱动目录包含 Win10／Win11 对应的 ZIP 和 SHA-256，支持为目标系统选择驱动。离线桌面宿主关闭 CET 兼容标志，避免受影响的旧版 Win10 在启动 CoreCLR 时崩溃。

在管理员 PowerShell 中，将 `$archive` 指向已校验的 ESD，再还原到空目录。

```powershell
$destination = New-Item -ItemType Directory -Path .\NetworkRescue
dism.exe /Apply-Image "/ImageFile:$archive" /Index:1 "/ApplyDir:$($destination.FullName)" /CheckIntegrity
```

普通 Windows 和 x64 WinPE 都可从还原目录打开 `NetworkRescue.exe`。普通 Windows 提供完整桌面急救窗口，WinPE 使用原生界面。用 `NetworkRescue.exe --gui` 显式打开图形窗口，或运行 `Run-Network-Rescue.cmd --cli` 在当前控制台操作。WinPE 修复现有 Windows 时，按目标系统构建号匹配签名 INF，经确认后通过 DISM 写入，重启进入目标系统后检查设备绑定及联网。

Hardware Check 的本地完整版曾在 Win10 22H2 和 Win11 24H2 断网来宾中还原，12,876 个文件的路径和 SHA-256 均与暂存目录一致，两台都打开了急救窗口。独立版 `c5a6be0` 的公开包已在 Win10 19045 和 Win11 26100 验证断网还原及 Realtek 8852CE 签名 INF 匹配。两台来宾的模拟设备安装后仍为 Code 28，实体网卡绑定和联网恢复待验证。

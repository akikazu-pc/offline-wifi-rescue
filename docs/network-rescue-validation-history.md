# 断网急救历史验证记录

2026-11-12 至 2026-11-13 的记录来自 Hardware Check 远端 main，涉及原项目的应用、发行包和实验环境。 [cb90eda 验证记录](https://github.com/akikazu-pc/hardware-check/blob/cb90eda75f33617fb38cf9bfe181e4adbfbaf02e/docs/validation-record.md)

## 2026-11-12 断网急救 WinPE 离线写入

在隔离的 Hyper-V 第 2 代虚拟机，用微软 Windows 11 Enterprise LTSC 评估介质启动 WinPE 10.0.26100.1742，将同一介质的 Windows 11 x64 映像应用至测试 VHDX 的 `W:`。启动脚本和驱动库来自离线完整版；复测使用修正后的原生 PE 助手。来宾通过根枚举测试设备提供 `PCI\VEN_10EC&DEV_8168&REV_15`，未连接虚拟交换机。

- 首测漏检目标系统。注册表 hive、AMD64 内核和版本资源可读，但 PE 的版本接口返回的文件主版本为 6.2、产品主版本为 10.0，内核构建号仍为 26100。
- 修正版本识别后，经 `Run-Network-Rescue.cmd` 进入 PE 修复，自动找到 `W:` 的 Windows 11 x64，键盘选择后匹配原厂签名 `rt68cx21x64.inf`。
- DISM 写入与离线驱动清单查询均返回 0。目标驱动存储列出 Realtek 网络驱动 `oem0.inf`，原始文件名为 `rt68cx21x64.inf`。
- 待验证：Windows 10 目标系统、实体网卡重启绑定和实际联网，以及全新安装阶段预装驱动。

## 2026-11-12 断网急救四语窗口与 Setup 启动

- 普通 Windows 便携窗口使用工具栏依次切换日语、繁体中文、英语和简体中文，核对窗口标题与工具栏标签。
- Hyper-V 来宾从独立 x64 WinPE 启动，使用键盘将向导切换到简体中文。内置字体正常显示中文，向导发现官方 Windows 11 x64 安装介质，按测试硬件 ID 匹配签名的 RTL8852BE 无线驱动，并带生成的应答文件启动官方 Windows Setup。
- 此次停在官方安装程序界面。完成安装、目标系统驱动存储、实体 Wi-Fi 绑定和首次开机联网仍待验收。

- 同一个 `NetworkRescue.exe` 在普通 Windows 打开便携窗口，在独立 x64 WinPE 虚拟机打开图形向导。

- 从独立 WinPE 的终端运行 EXE，确认打开命令行并在无按键时继续；按键后显示语言编号。在普通 Windows 以重定向输出验证默认简体中文。精简英文 WinPE 控制台缺少 CJK 字体，图形向导用于该环境的中日文显示。

- 微软 Windows 11 安装介质的 WinPE 中，命令行自动选中唯一的 Windows 11 和签名 RTL8852BE 驱动，编号确认后 DISM 写入成功，驱动存储列出 `oem1.inf`（`netrtwlane601.inf`）。测试使用根枚举硬件 ID。独立精简 PE 启动映像可以扫描，但该环境的 DISM 返回 `0x80004002`，写入改在安装介质的 PE 中验证。

## 断网急救共享图形窗口

- 普通 Windows 与两种 x64 WinPE 运行同一个原生图形窗口。普通 Windows 核对四语标题和工具栏选项，并打开完整急救工具。
- 独立 WinPE 验证键盘切换简体中文、列表显示离线 Windows 11，并通过「工具」进入修复目标选择。
- 微软安装介质的 PE 用 Shift+F10 执行同一 EXE 的图形模式，窗口正常显示并列出离线 Windows 11。

## 2026-11-12 断网急救 WinPE 窗口布局

- 使用同一个便携启动器，在 Hyper-V 的独立 x64 WinPE 和微软 Windows 11 安装介质中通过键盘打开新窗口。800×600 下可见顶部工具栏、本机网络状态、驱动准备按钮和自动发现的 Windows 11 测试分区。
- 独立 WinPE 中使用键盘切换简体中文，确认顶部工具栏、列表列名和状态更新。安装介质中按 Enter 打开驱动准备，确认已启动安装器的提示正常显示。此轮验证覆盖界面启动与操作，未重新执行驱动写入或 Wi-Fi 联网。
- 后续在同一 Hyper-V 环境复测两种 WinPE，均确认标题栏与主窗口显示共享应用图标，本机网络和驱动准备标题旁显示系统图标。独立 WinPE 使用简体中文，微软安装介质使用英语。此轮只复测界面。
- 再次调整时移除窗口内容区图标，保留标题栏图标。两种 WinPE 均确认新版顶部下拉工具栏、网络列表及驱动准备区可见。独立 WinPE 用键盘切换简体中文、打开工具下拉菜单和关于对话框，微软安装介质确认安装器已运行时的提示仍可打开。
- 随后将顶部改为 Windows 原生 `ToolbarWindow32`。独立 WinPE 中以 Tab、方向键打开语言列表并切换简体中文，微软 Windows 11 安装介质中以鼠标打开工具列表。工具栏文字及选择列表使用内置字体，此轮只复测界面。

## 断网急救桌面网络诊断

- 在未连接虚拟交换机的 Hyper-V Windows 11 首次设置环境，通过离线启动器打开完整桌面窗口，确认内核调试适配器不会显示为实体网卡。切换简体中文后，点击「复测连接」和「检测网络故障」，窗口仍正确报告未检测到实体网卡或缺驱动候选。
- 自动化测试覆盖无链路、无可用 IP、无网关、多网卡、Windows 报告互联网状态以及有线设备错误的区分。本轮未对实体网卡执行驱动安装，也未重新验证 PE 写入和全新安装后的联网。
- 后续在隔离的 Windows 11 Hyper-V 差分磁盘中，用合成设备模拟问题码 28，并在空驱动库中点击「安装网络驱动」。验证替代来源窗口出现，能打开本地目录选择器或进入线上候选列表。此次测试未下载、安装驱动或恢复联网。
- 在 Windows 11 Hyper-V 来宾用两个不同硬件 ID 的合成网卡模拟缺驱动，签名 `e2fn.inf` 同时匹配 I226-V 与 I226-LM。图形窗口列出两块网卡；选择 I226-LM 后，确认窗口只列出 `PCI\VEN_8086&DEV_125B` 及签名者。验证停在确认界面。

## 2026-11-13 Hyper-V 消费版断网部署与修复

- 环境：本机 Hyper-V 第 2 代来宾，开启 Secure Boot，无虚拟 TPM，不接网卡。介质为微软官网的 Windows 11 26300 与 Windows 10 22H2 日语多版本 ISO，SHA-256 与官网公布值一致。PE 程序由 `main` 构建，有线驱动取自本地离线完整版 ZIP。
- 缺驱动网卡用实验专用的根枚举设备模拟：硬件 ID 和兼容 ID 与实体 I226-V 或 RTL8168 相同，未装驱动，问题码 28。此方式不能验证真实硬件加载和联网。

| 场景 | 方式 | 自动选择的签名 INF | 结果 |
| --- | --- | --- | --- |
| Windows 11 Home 26300 | PE 无人值守新装，I226-V | `e2fn.inf` 2.1.5.7 | 入库为 `oem0.inf`，用户确认许可后离线进入 `user` 桌面 |
| Windows 11 Pro 26300 | PE 无人值守新装，RTL8168 | `rt68cx21x64.inf` 1168.029.50.0202 | 入库为 `oem0.inf`，用户确认许可后离线进入 `user` 桌面 |
| Windows 10 Pro 22H2（19045.2965） | PE 无人值守新装，I226-V | `e2f.inf` 1.1.4.38 | 入库为 `oem0.inf`，用户确认许可后离线进入 `user` 桌面 |
| Windows 10 Home 22H2 | PE 无人值守新装，RTL8168 | `rt640x64.inf` 10.79.50.1003 | 入库为 `oem0.inf`，用户确认许可后离线进入 `user` 桌面 |
| 已有 Windows 10 Pro | PE 修复，RTL8168 | `rt640x64.inf` 10.79.50.1003 | DISM 写入为 `oem3.inf` |
| 运行中的 Windows 11 LTSC 24H2 | 命令行修复，I226-V | `e2fn.inf` 2.1.5.7 | 修复前返回 `NoEligibleDevice`，修复后驱动安装到设备并启用 `e2fnexpress` 服务，报告「绑定未确认」 |

- 四次新装均为 GPT：EFI 260 MiB、MSR 16 MiB、C 盘 98.73 GiB、恢复分区 1 GiB，Windows RE 已注册，应答文件已写入。四台均在无网络时进入日语微软许可页。许可需用户本人确认。四台确认许可后均无其他页面，离线自动进入日语桌面；`UnattendGC` 日志均记录创建 `user`，用户目录已生成，部署时写入的网卡驱动仍在驱动库。
- 修复的缺陷：运行中 Windows 的安装器只按硬件 ID 查找设备，而 `e2fn.inf` 匹配的 `PCI\VEN_8086&DEV_125C` 是 Windows 报告的兼容 ID，因此 I226-V 无法安装。已改为同时比对兼容 ID，与 `UpdateDriverForPlugAndPlayDevices` 一致，仍要求恰好一台问题码 28 的设备。另修正命令行版本列表重复编号。
- 合成设备无真实硬件，驱动加载后为问题码 31，所以结果是「绑定未确认」。实体网卡的绑定、地址、DNS 和 HTTPS 仍需按[实机验收](network-rescue-field-test.md)执行。
- Windows 11 26300 和 Windows 10 22H2 映像自带 Realtek `rt640x64.inf`，常见 RTL8168 系列可能无需额外驱动即可联网。

## 2026-11-15 独立版公开 ESD 的 Code 28 排查与复测

在两台未连接虚拟交换机的 Hyper-V 差分来宾中，原根枚举测试设备报告了 Realtek 8852CE 硬件 ID `PCI\VEN_10EC&DEV_C852&SUBSYS_C85210EC`，但没有报告 PCI 无线类别兼容 ID。原生命令行匹配到了已签名的 `netrtwlane602.inf`，桌面安装器的设备资格检查返回 `NoEligibleDevice`（3），没有调用 Windows 驱动安装。两台来宾因此保持 Code 28。

原生命令行和图形安装入口现在分别显示设备不符合安装条件、驱动包校验失败等结果。安装资格检查保留。重打公开 ESD 后，两台来宾均核对了 SHA-256，并用系统 DISM 在断网环境还原。

| 来宾 | 原测试设备 | 补齐 `PCI\CC_028000` 和 `PCI\CC_0280` 后 | 驱动入库 |
| --- | --- | --- | --- |
| Windows 10 Pro，19045 | 匹配该 INF，CLI 返回 3 并提示设备不符合条件 | 安装器返回「绑定未确认」（2），设备为 Code 31，已登记服务 `rtwlane602` | `pnputil /add-driver` 返回 0 |
| Windows 11 Enterprise LTSC，26100 | 匹配该 INF，CLI 返回 3 并提示设备不符合条件 | 安装器返回「绑定未确认」（2），设备为 Code 31，已登记服务 `rtwlane602` | `pnputil /add-driver` 返回 0 |

复测包 `NetworkRescue-0.2.1-offline-redistributable-win-x64.esd` 的 SHA-256 为 `10A44E9293A2B583D1451C517D985252333E58F442ACCC241F383FA2B94555C4`。本地证据为 `artifacts/verify-main-014945/OWR-c5a6be0-W10-results/retest.txt` 和 `artifacts/verify-main-014945/OWR-c5a6be0-W11-results/retest.txt`，同目录的 `setupapi.dev.log` 记录 Windows 驱动安装过程。

**待验证**。用公开包支持的实体 Realtek 网卡验证驱动安装、设备正常启动和联网恢复，包括地址、DNS 和 HTTPS。宿主机的 Intel I226-V 与 MediaTek MT7925 均不匹配本包驱动。 [实机验收](network-rescue-field-test.md)

## 2026-11-15 迁移后的 Hyper-V 介质与模拟网卡

- 本地归档位于 `artifacts/hardware-check-shared-desktop-archive/pe-hyperv-validation/`。迁移时逐文件核对了相对路径、大小和 SHA-256。更新路径后，`consumer-lab/tools/Lab.psm1` 可加载并解析到新归档，归档中的 PowerShell 脚本通过语法检查。
- 挂载 `consumer-media/` 下的 Win10 22H2 和 Win11 26300 安装 ISO，均可读取 `sources/boot.wim` 和安装映像。用归档中的 `matched-setup-pe.iso` 启动隔离的 Hyper-V 第 2 代来宾 `OWR-ArchiveMedia-Check`，成功进入 WinPE 命令提示符。
- 在该来宾挂载 `lab-bin4.iso`，运行其中的 `MissingNicFixture.exe`。工具注册返回 0，`pnputil /enum-devices /problem 28` 确认 `ROOT\NET\0000` 为 Code 28（`CM_PROB_FAILED_INSTALL`）。测试后已关闭来宾。

本轮覆盖介质挂载、WinPE 启动和模拟设备注册。实体网卡安装、绑定和联网恢复仍待验证。截图保存在本地 `artifacts/archive-media-check.png`。

## 2026-11-15 公开离线 ESD 双系统复测

从 `offline-wifi-rescue` 的 `main` 提交 `0a29e54` 重打公开离线包。ESD 的 SHA-256 为 `0E79D66B9143AB11841E0D93DCF1C45B1645F8FFB77974C04B31E1053A8342EE`。

用新归档的 `matched-setup-pe.iso` 再次启动 WinPE。从同一归档的 `lab-bin4.iso` 取出 `MissingNicFixture.exe`，在两个未接虚拟交换机的 Hyper-V 差分来宾中创建 Realtek 8852CE 模拟设备。测试设备报告 `PCI\VEN_10EC&DEV_C852&SUBSYS_C85210EC` 及 PCI 无线类别兼容 ID。运行前，两台来宾均确认设备为 Code 28，驱动库中没有目标 INF。

| 来宾 | 校验与断网解包 | 签名 INF 匹配 | 驱动入库 |
| --- | --- | --- | --- |
| Windows 10 Pro，19045 | SHA-256 一致，DISM `/Apply-Image /CheckIntegrity` 成功 | `netrtwlane602.inf`，版本 `6001.16.172.0` | 通过，急救命令行安装后新增 `oem0.inf` |
| Windows 11 Enterprise LTSC Evaluation，26100 | SHA-256 一致，DISM `/Apply-Image /CheckIntegrity` 成功 | `netrtwlane602.inf`，版本 `6001.16.172.0` | 通过，急救命令行安装后新增 `oem0.inf` |

两台来宾均用 `pnputil /enum-drivers` 和 `setupapi.dev.log` 核对入库结果。模拟设备安装后均为 Code 31，CLI 返回 2（绑定未确认）。来宾已关机，日志保留在本地 `artifacts/archive-public-retest-0a29e54/`。**实体网卡安装、正常绑定和联网恢复仍待验证。**

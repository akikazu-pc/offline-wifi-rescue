# 调研与首版设计

## 同类产品

| 产品 | 官网描述的能力 | 对项目的启发 |
| --- | --- | --- |
| [Snappy Driver Installer Origin](https://www.snappy-driver-installer.org/index.html) | 通用离线驱动安装、驱动包更新 | 提供预先准备好的 Wi-Fi 急救包，用公开的硬件覆盖记录帮助用户选择 |
| [3DP Net](https://www.3dpchip.com/3dpchip/index_eng.html) | 从集成网络驱动池检测并选择驱动，重装后先恢复网络 | 网络急救是成熟的使用场景，操作可围绕扫描、确认和安装展开 |
| [Driver Booster Offline Driver Updater](https://www.iobit.com/en/knowledge-offline-driver-updater-instructions-293.php) | 导出设备信息、在联网电脑下载、带回离线安装 | 增加按设备导出请求的入口，覆盖预制包缺项 |
| [Dell 企业驱动包](https://www.dell.com/support/kbdoc/en-us/000180534/dell-family-driver-packs) | 按设备家族和 Windows 版本提供部署包 | 笔记本可按 OEM 优先取包，但仍须校验实际硬件和 INF |

竞品功能来自厂商文档，本轮未实测。两条救援路径已有先例，本项目可在 Wi-Fi 专项覆盖、包来源可追溯和安装结果可核查上做深。

## 硬件覆盖

技嘉 B650 AORUS ELITE AX 的 rev. 1.0、1.1、1.2 分别用 RZ616、AX210、RTL8852CE。主板型号适合查厂商资料，匹配驱动时应读取 PCI 硬件 ID 和 SUBSYS，后续 USB 适配器可使用 VID/PID。笔记本同时考虑 OEM 机型限制及 INF 依赖。以 Windows 的匹配和排名为准。参考[技嘉主板手册](https://download.gigabyte.com/FileList/Manual/mb_manual_b650-aorus-elite-ax_1203_e.pdf)和[微软驱动选择流程](https://learn.microsoft.com/en-us/windows-hardware/drivers/install/overview-of-the-driver-selection-process)。

旧平台先抽样 LGA1150、LGA1151、LGA1200 和 AM4 早期型号。华硕 Z97I-PLUS 有 Windows 10 无线驱动，Intel 的旧驱动发布说明也分别列出 7260、3160、7265 系列。样本能说明存在多代旧模块和独立维护分支，尚不足以估计各平台的网卡数量或 Windows 自带驱动比例。参考[华硕支持页](https://www.asus.com/supportonly/z97iplus/helpdesk_download/)和[Intel 发布说明](https://community.intel.com/cipcp26785/attachments/cipcp26785/wireless/48379/2/ReleaseNotes_WiFi_22.170.0.pdf)。

验证免手动安装时，记录主板修订版、硬件 ID、Windows ISO 版本及构建号。使用干净微软镜像、全程断网，在安装项目驱动前检查网卡能否扫描并连入本地接入点。另记联网后 Windows Update 能否补装、OEM 恢复镜像是否预装。可用 DISM 辅助核对镜像驱动清单。参考[微软设备驱动匹配示例](https://learn.microsoft.com/en-gb/windows-hardware/drivers/install/step-2--a-driver-for-the-device-is-selected)和[DISM 文档](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-driver-servicing-command-line-options-s14?view=windows-11)。

Windows 10 和 11 分别记录驱动可用性。Intel 通用包已移除 AX200，需要保留按型号查找历史包的路径。网卡可用与系统支持 Wi-Fi 7 特性也应单独验证。参考[Intel 当前驱动页](https://www.intel.com/content/www/us/en/download/19351/intel-wireless-wi-fi-drivers-for-windows-10-and-windows-11.html)和[Intel Wi-Fi 7 发布说明](https://downloadmirror.intel.com/830389/ReleaseNotes_WiFi_23.70.2.pdf)。

## 首版

1. 扫描已识别的网卡和没有驱动的设备。区分缺驱动、设备未枚举、已有可用驱动、安装后需重启。扫描无需提权，确认安装时提权。
2. 两种使用方式。预先用联网电脑准备 U 盘离线包，或在离线电脑导出最小设备清单，再去联网电脑下载对应包。清单包含硬件 ID 和系统构建，排除序列号、MAC、Wi-Fi 名称及密码。
3. 显示驱动来源、版本、适用系统和匹配依据。校验包哈希和 Windows 驱动签名，再通过 PnPUtil 安装匹配 INF。检查实际绑定的驱动和无线接口状态。驱动已入库、已绑定、能扫描 Wi-Fi、已联网分别显示。默认保留已正常工作的驱动。参考[PnPUtil 命令文档](https://learn.microsoft.com/en-us/windows-hardware/drivers/devtest/pnputil-command-syntax)。
4. 驱动索引记录厂商链接、硬件 ID、架构、系统范围、版本、哈希、授权状态和实测证据。每个包单独核对再分发权限。未确认授权的包由用户在联网端从官方来源获取，可作为本地包导入。
5. 首发先验证四个平台的台式机代表样本，再用不同 OEM 的笔记本验证选包逻辑。覆盖表区分官方资料确认、INF 匹配、实机安装联网三种证据。首轮可选 Intel、MediaTek、Realtek、Qualcomm 的代表型号。

后续功能优先考虑重装前导出当前驱动、常见有线网卡作为备用联网路径，以及安装失败时的日志与恢复指引。ARM64 笔记本、老旧 USB Wi-Fi 和 Windows 初始设置阶段应有独立验证。

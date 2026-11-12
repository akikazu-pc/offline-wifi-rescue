# 台式机版第一阶段覆盖验收

第一阶段覆盖 AM4、AM5、LGA1700、LGA1851 的公开可查组装机主板，涵盖各品牌、停售型号、地区后缀、PCB 修订和有证据的出厂替换模块。主板清单也收录无板载 Wi-Fi、仅预留接口和选装模块的产品。后装网卡按其设备 ID 单独收录。

## 来源核查

| 来源 | 可用于 | 证据边界 |
| --- | --- | --- |
| [IT 天空万能驱动入口](https://www.itsk.com/ed) | 参考断网扫描、集成驱动包及安装交互 | 本轮入口返回 403。用户反馈曾遇到无法找到对应网卡的情况，具体版本和原因待复现 |
| [SDIO 项目](https://sourceforge.net/projects/snappy-driver-installer-origin/)与 [sdio-go](https://github.com/IDisposable/sdio-go) | 参考硬件检测、驱动索引、候选匹配及离线安装 | 工具与索引无法证明各主板的出厂配卡，也不能证明第三方驱动再分发授权 |
| [FirPE DriverIndexer](https://github.com/FirPE-Team/DriverIndexer) | 参考 INF 索引、包指纹、离线匹配 | GPLv3。当前 MIT 项目可参考设计，自行实现 |
| [pciutils/pciids](https://github.com/pciutils/pciids) | 将 PCI 厂商、设备和子系统 ID 转为可读名称 | 提供设备名称线索。驱动适用性要核对 Windows INF |
| [linuxhw/hw-probe](https://github.com/linuxhw/hw-probe) | 从实机探测寻找主板与 PCI 无线设备的组合 | 用户可能换过网卡。单条探测只能证明该台机器出现过该组合 |
| [Microsoft Update Catalog](https://www.catalog.update.microsoft.com/) | 找厂商发布的 Windows 候选驱动，补查官网缺项 | 逐包核对 Hardware IDs、Windows 构建、签名和许可 |

本轮核查的资料中没有找到同时提供四个平台全品牌主板、出厂网卡及两套 Windows 驱动的可用开源数据库。板型发现以各厂商产品、支持和历史存档为主，跨品牌目录与社区探测用于补漏。记录各品牌和区域的入口、快照日期以及漏项，覆盖率须注明统计分母。

## 逐板记录与发布条件

1. 每条主板记录有品牌、型号、平台、区域、PCB 修订、出厂 Wi-Fi 状态和证据链接。有多种出厂模块就逐一登记。仅标注 Wi-Fi 6E 或 Wi-Fi 7 的记录继续追查到具体模块，使用认证文件、厂商资料或可追溯的实机记录交叉核实。
2. 每个模块记录型号、PCI VEN/DEV 与适用 SUBSYS，或对应总线的设备 ID。Windows 10 x64 和 Windows 11 x64 分开核对。可用驱动记录包来源、版本、INF、适用系统构建、签名与 SHA-256。明确缺少受支持驱动的组合须注明依据并显示系统限制，不能计入可离线安装数量。
3. 台式机版发布前，已纳入快照的型号不得留有未知 Wi-Fi 状态、未知出厂模块或适用性未核对的驱动。维护按平台、品牌和系统的缺项清单。官方规格确认、包与 INF 验证、实机联网是独立的证据状态。
4. 扫描端用实际设备 ID 匹配，主板型号用于解释和排查。界面明确区分设备未枚举、设备已识别但包中没有驱动、系统版本无适用驱动。安装测试至少覆盖每类驱动包与目标系统，记录当前驱动绑定以及能否连入本地接入点。

## 首批证据样例

| 平台与主板 | 已核对的出厂模块证据 | 驱动与待核项 |
| --- | --- | --- |
| AM4 · 技嘉 A520I AC | 1.0 / 1.2 / 1.3 为 Intel AC 3168，1.1 为 Intel AC 9260，1.4 / 1.5 为 Realtek RTL8821CE。[手册](https://download.gigabyte.com/FileList/Manual/mb_manual_a520i-ac_1402_e.pdf) · [1.5 规格](https://www.gigabyte.com/de/Motherboard/A520I-AC-rev-15/sp) | [1.5 官方下载](https://www.gigabyte.com/id/Motherboard/A520I-AC-rev-15/support)及 [RTL8821CE Catalog](https://www.catalog.update.microsoft.com/Search.aspx?q=Realtek+RTL8821CE+Wireless+LAN+Driver)供查包，仍须逐版本核 INF |
| AM5 · 技嘉 B650 AORUS ELITE AX | 1.0 为 RZ616，1.1 为 AX210，1.2 为 RTL8852CE。[手册](https://download.gigabyte.com/FileList/Manual/mb_manual_b650-aorus-elite-ax_1203_e.pdf) | 技嘉分别列出 AMD、Intel、Realtek 无线驱动。[1.0 / 1.1](https://www.gigabyte.com/Motherboard/B650-AORUS-ELITE-AX-rev-10-11/support) · [1.2](https://www.gigabyte.com/Motherboard/B650-AORUS-ELITE-AX-rev-12/Support/sp)。仍须核 INF |
| LGA1700 · 华擎 B760M PG SONIC WiFi | [官网规格](https://pg.asrock.com/mb/Intel/B760M%20PG%20SONIC%20WiFi/Specification.asp)只标 Wi-Fi 6E | 模块和驱动对应仍待确认 |
| LGA1851 · 技嘉 Z890 EAGLE WIFI7 | [官网规格](https://www.gigabyte.com/se/Motherboard/Z890-EAGLE-WIFI7/sp)列有 MediaTek MT7925 | 仍须按修订版核齐配置，并分别验证 Windows 10/11 的驱动 |

本轮完成来源核查和样例。四个平台的主板总表、驱动包 INF 校验和实机测试尚未完成。

## 多来源交叉核对

- **找齐主板。** 合并各地区厂商产品页、支持目录、停售存档及第三方目录的候选清单。按品牌、型号、区域、修订去重，保留每条记录的发现来源和日期。统计目录已确认率、网卡已确认率以及 Win10 / Win11 驱动覆盖率，分别列出未核项。
- **确认出厂模块。** 优先使用明确对应修订的厂商规格、手册或认证文件。厂商规格只写 Wi-Fi 世代时，用可追溯的原装实机、认证文件和厂商驱动候选交叉核对。多篇转载同一资料算同一来源。出现冲突时先检查地区、PCB 修订与是否更换过网卡，未解冲突保留为待核。
- **确认驱动可用。** 将实际设备 ID、适用 SUBSYS 和目标 Windows 构建与官方驱动包 INF 对照，记录签名及哈希。主板对应关系和驱动适用性各自保留证据。官网缺包时再查芯片厂及 Microsoft Update Catalog。软件安装后复查驱动绑定和设备状态。

## 目录快照与发布判定

每次验收冻结一份公开可查目录及日期。保留尚未核查的厂商或区域入口，新增、纠错和后续修订进入下一版快照。用主板变体、出厂模块证据、实际设备标识、目标 Windows 构建与驱动包四组记录串联来源。主板目录完成率、出厂模块确认率、Windows 10/11 离线可安装率分别公布分子和分母。仅可自行下载、授权未确认、包缺失及厂商未提供适用驱动的组合分别标注。无适用包须留查询来源和日期。有 Wi-Fi 的变体仍缺模块、INF 证据或可获取离线包时，列入对应发布缺口；适用系统确无驱动的组合公开列为限制。

实机回归优先覆盖同型号不同修订换卡、设备未枚举、已识别但包缺失、候选已入库但设备未换绑。连接本地接入点与互联网可达分别验证。

## 新型号优先

台式机先核查 AM5 和 LGA1851，笔记本先核查 2025 年起出货的新型号。按品牌记录已发现的型号、出厂网卡变体及 Windows 10/11 支持状态。硬件匹配率只统计有确切 ID 的样本，资料中没有支持驱动的组合单独公开。先补齐新型号的实际 ID、签名 INF 和缺驱动实机验证，再系统扩大到 AM4、LGA1700 和更旧的笔记本。

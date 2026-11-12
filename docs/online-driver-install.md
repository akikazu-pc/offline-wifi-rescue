# 恢复联网后的驱动安装

第一阶段通过离线包恢复 Wi-Fi。下一阶段在同一个工具里提供「检查缺失驱动」，用户可以在 Wi-Fi 恢复或通过有线等方式接入互联网后使用。

1. 本地扫描缺少驱动的设备，按硬件 ID、子系统 ID、Windows 构建、架构及整机型号检索候选。优先支持网络、芯片组及常见缺驱动设备，台式机与笔记本共用流程。
2. 下载带签名的项目驱动索引。索引记录官方地址、包版本、适用条件及校验信息。客户端按需从 OEM、芯片厂或微软获取驱动。评估用 Windows Update Agent 查询和安装适用更新，遵守系统更新策略。项目索引首版可用静态发布实现。
3. 列出将安装的设备、来源与版本。用户确认一次后自动下载和校验，再按包依赖顺序安装。已正常工作的驱动保留，正在联网的网卡默认跳过。BIOS 与固件更新留待独立设计。
4. 下载完成后核对哈希、签名及 INF 适用性。通过 Windows 驱动安装机制执行并复查绑定，显示成功、失败、仍缺驱动和需重启的设备。保留本地日志，支持失败重试；能导出的现有驱动可在替换前备份。
5. 用干净安装的 Windows 10 / 11 测试离线恢复 Wi-Fi 后接续安装，并覆盖下载中断、无候选、签名异常和需要重启的路径。

参考[Windows Update Agent API](https://learn.microsoft.com/windows/win32/wua_sdk/portal-client)、[Windows 驱动排名](https://learn.microsoft.com/en-us/windows-hardware/drivers/install/how-windows-ranks-driver-packages)及 [PnPUtil](https://learn.microsoft.com/en-us/windows-hardware/drivers/devtest/pnputil-command-syntax)。

补装缺失驱动和升级现有驱动使用独立入口。升级、备份回退和安装包规划见[发布形式与驱动升级](distribution-and-updates.md)。

# 断网急救发行清单

- 核对 `Directory.Build.props` 的应用版本与预期发行版本。在准备发布的提交上构建桌面版及 WinPE，运行核心测试。
- 在管理员 PowerShell 中用下面的命令构建公开 ESD。将两个变量指向本地输入，核对原厂 ZIP 的文件名、SHA-256 和允许随包分发的文件清单。保留驱动签名和版权声明。 [公开驱动清单](../packaging/shared-desktop/public-offline-drivers.json)

```powershell
pwsh -NoProfile -File scripts/publish-network-rescue-bundles.ps1 -PublicOffline -RealtekVendorZip $realtek8852Zip -PublicDriverSourceDirectory $additionalDriverDirectory
```

- 从 `artifacts/network-rescue` 核对 ESD 小于 2 GiB，计算 SHA-256 并与同名校验文件比对。构建脚本用 DISM 还原 ESD，并逐文件核对路径和 SHA-256。
- 在断网的 Win10 和 Win11 x64 来宾分别用系统 DISM 还原，核对还原文件，启动图形窗口，记录系统构建号、包哈希和匹配到的签名 INF。
- 用兼容网卡核对实际绑定的 INF、驱动版本和设备状态。记录重启要求，并在安装后验证地址、默认路由及 HTTPS。测试设备只用于提供硬件 ID 时，单独记录匹配结果。
- 发布后从实际公开下载链接重新下载 ESD 和校验文件，再计算 SHA-256。若下载页另有版本和校验值，同步核对。

公开 ESD 的检查项来自 Hardware Check 本地提交 `fdd8df6`，并按独立版的输入、产物和验收流程整理。

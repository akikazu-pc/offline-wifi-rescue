# 型号库迁移与索引重建

型号、证据和待核台账来自 Hardware Check 的 `fdd8df6`。来源提交保留在原仓库。独立项目中的 `docs/catalog-inputs/` 保存索引导出器读取的 OEM 元数据快照。驱动系统对照表仅包含索引所需的下载来源和 Windows 版本。

在本仓库根目录运行：

```powershell
python scripts/export-network-rescue-search.py --output rebuilt-search-models.csv
Get-FileHash rebuilt-search-models.csv, docs/network-rescue-search-models.csv
```

此次迁移使用的索引 SHA-256 为 `8865c41afdbbedc51aa2ef519ebc33c5caf510999e4ef7dc76e15829efed8c84`，共 39,630 条。更新来源快照时，保持主板、笔记本和其他设备的快照版本一致，并核对重建索引的逐行变化。

[待核台账](network-rescue-model-evidence-pending.csv)保留原始来源链接和历史查证结论。其缓存路径指向原硬件检测仓库的本地审查资料。补证后同步更新[型号库](network-rescue-driverhub-model-candidates.csv)、[证据明细](network-rescue-model-evidence-backfill.csv)和搜索索引，再从待核台账移除对应记录。

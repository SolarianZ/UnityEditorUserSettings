# EditorUserSettings

存储用于Unity Editor环境的自定义配置数据，可以控制是否在项目间共享这些配置数据。<br/>Store custom settings data for Unity Editor environment, and control whether to share these settings data between projects.


## 支持的Unity版本<br/>Supported Unity Versions

Unity 2019.4及更新版本。<br/>Unity 2019.4 or higher.


## 安装方式<br/>Installation

[![openupm](https://img.shields.io/npm/v/com.greenbamboogames.editorusersettings?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.greenbamboogames.editorusersettings/)

使用 [OpenUPM](https://openupm.com/packages/com.greenbamboogames.editorusersettings) 安装此包，或者直接克隆此仓库到项目中。<br/>Install this package via [OpenUPM](https://openupm.com/packages/com.greenbamboogames.editorusersettings), or clone this repository directly into the Packages folder of your project.


## 如何使用<br/>How to Use

### API

```csharp
static class CustomEditorUserSettings
{
    bool Has<T>(string key, bool isSharedAcrossProjects = false);

    T Get<T>(string key, T defaultValue, bool isSharedAcrossProjects = false);
    bool TryGet<T>(string key, out T value);

    void Set<T>(string key, T value, bool isSharedAcrossProjects = false);

    bool Remove<T>(string key, bool isSharedAcrossProjects = false);
    bool RemoveAll<T>(bool isSharedAcrossProjects = false);
  
    void Clear(bool isSharedAcrossProjects = false);

    BatchingScope StartBatching(bool isSharedAcrossProjects = false);
    void EndBatching(bool isSharedAcrossProjects = false);
}
```


### 菜单项<br/>Menu Items

- Tools/Bamboo/Editor User Settings
    - 检视项目配置存储对象：Inspect Storage Object for Project
    - 在文件夹中显示项目配置存储对象：Show Storage Object for Project in Folder
    - 检视跨项目配置存储对象：Inspect Storage Object Shared Across Projects
    - 在文件夹中显示跨项目配置存储对象：Show Storage Object Shared Across Projects in Folder


## 已知问题<br/>Known Issues

1. 读写跨项目配置数据时，没有添加文件锁机制，同时读写时，可能会导致数据丢失。<br/>When reading and writing data across projects, there is no file lock mechanism added, and data may be lost when reading and writing at the same time.
2. 读写跨项目配置数据时，低版本Unity可能无法读取某些高版本Unity存储的数据。<br/> When reading and writing data across projects, lower versions of Unity may not be able to read some data stored by higher versions of Unity.
<p></p>
<p></p>
<p align="center">
<img src="https://img-blog.csdnimg.cn/20210406140816743.png" width = "100" height = "100" alt="图片名称" align=center />
</p>

 <div align="center"> 
  
[![NuGet version (RRQMSocket)](https://img.shields.io/nuget/v/RRQMSocket.svg?style=flat-square)](https://www.nuget.org/packages?q=rrqm)
[![License](https://img.shields.io/badge/license-Apache%202-4EB1BA.svg)](https://www.apache.org/licenses/LICENSE-2.0.html)
[![Download](https://img.shields.io/nuget/dt/RRQMSocket)](https://img.shields.io/nuget/dt/RRQMSocket)
[![star](https://gitee.com/dotnetchina/RRQMSocket/badge/star.svg?theme=gvp)](https://gitee.com/dotnetchina/RRQMSocket/stargazers) 
[![fork](https://gitee.com/dotnetchina/RRQMSocket/badge/fork.svg?theme=gvp)](https://gitee.com/dotnetchina/RRQMSocket/members)

</div> 

<div align="center">

日月之行，若出其中；星汉灿烂，若出其里。

</div>

## 💿描述
&emsp;&emsp;RRQMSocket是一个整合性的、超轻量级的网络通信框架。支持C#语言系全平台（.NET Framework4.5及以上，.NET Core3.1及以上，.net standard2.0及以上），支持C#语言系全框架（WPF、Winform、Blazor、Xamarin、Mono、Unity等）。它具有高并发、事件订阅、插件式扩展、自动活性检测、多线程处理等特点，让使用者能够更加简单的、快速的搭建网络框架。

&emsp;&emsp;目前RRQMSocket支持的传输层协议有TCP和UDP两款，基于这两大协议的支持，使得RRQMSocket能够解析绝大部分应用层协议，比如耳熟能详的HTTP、FTP、Telnet、POP3、SMTP、DNS等，不仅如此，RRQMSocket还内置了多种分包算法，使跨语言解析变得更加简单快捷。

&emsp;&emsp;RRQMSocket内部设计大量引用了内存池、对象池、等待池、线程池等众多“池”化设计，使得整个框架在接收和发送数据时基本上避免了“创建-销毁”的性能消耗。而且在接收数据时设计了完美的IOCP模型，不仅避免了接收数据的再复制行为，还让数据接收与数据处理分线程操作，大大减轻了内核的负担，让内核专注于接收，性能由此提升不少。这些优化行为的最终结果就是让RRQMSocket拥有超强的数据接收处理能力，这也就意味着它的适用场景也更加广泛，您可以用它开发分布式服务器、游戏服务器、文件管理服务器、即时通信服务器等。我相信，它的表现一定令您满意。

&emsp;&emsp;除了基本的框架外，程序集内还开发出了一些成熟框架，目前有`Token服务器`、`文件传输框架`、`RPC框架`两种，后续还会不断更新添加其他框架，希望大家多多支持和关注。

### 🔗联系作者

 - [CSDN博客主页](https://blog.csdn.net/qq_40374647)
 - [哔哩哔哩视频](https://space.bilibili.com/94253567)
 - [源代码仓库主页](https://gitee.com/RRQM_Home) 
 - 交流QQ群：234762506

### ✨API文档

[RRQMSocket API文档](https://gitee.com/dotnetchina/RRQMSocket/wikis/pages)

（TCP、UDP、文件传输、RPC文档已完善。其他扩展件目前还在积极完善当中）

 ### 🖥支持环境
- .NET Framework4.5及以上。
- .NET Core3.1及以上。
- .NET Standard2.0及以上。

### 📦 安装

- [Nuget RRQMSocket](https://www.nuget.org/packages/RRQMSocket/)
- [微软Nuget安装教程](https://docs.microsoft.com/zh-cn/nuget/quickstart/install-and-use-a-package-in-visual-studio)

## 一、TCP框架
#### 1.1 创建TCP框架
几行代码就可以搭建出完整的TCP高性能框架，具体创建步骤详见[RRQMSocket创建高并发、高性能TCP框架](https://blog.csdn.net/qq_40374647/article/details/110679663?spm=1001.2014.3001.5501)。

#### 1.2 特点
- 简单易用。
- 多线程处理。
- IOCP完美设计模型，避免收到数据再复制。
- 简单、稳定管理客户端连接，自动检验客户端活性。
- 超简单的解决粘包、分包问题，详见[RRQMSocket解决TCP粘包、分包问题](https://blog.csdn.net/qq_40374647/article/details/110680179?spm=1001.2014.3001.5501)。
- 内存池设计，避免内存重复申请、释放。
- 对象池设计，避免数据对象的申请、释放。


## 二、Token系服务器
#### 2.1 概述
Token系服务器是基于Tcp服务器一款限定连接的服务器，其主要功能就是对即将完成连接的客户端进行筛选，筛选手段就是验证Token，如果Token不符合规定，则直接断开连接，其他功能和Tcp服务器一致。


## 三、文件传输框架
#### 3.1 创建文件服务器框架
几行代码就可以搭建出完整的高性能文件传输框架，具体创建步骤详见[RRQMSocket创建文件传输、大文件续传框架](https://blog.csdn.net/qq_40374647/article/details/100546120?spm=1001.2014.3001.5501)。

#### 3.2 特点
- 简单易用。
- 多线程处理。
- 高性能，传输速度可达500Mb/s。
- 超简单的传输限速设置，1k-10Gb 无级调节。
- 超简单的传输速度、传输进度获取。
- 随心所欲的暂停、继续、停止传输。
- 系统化的权限管理，让敏感文件只允许私有化下载。
- 随时发送消息，让客户端和服务器交流不延迟。
- 基于事件驱动，让每一步操作尽在掌握。
- 可视化的文件块流，可以实现像迅雷一样的填充式进度条。
- 超简单的断点续传设置，为大文件传输保驾护航。
- 无状态上传断点续传设置，让同一个文件，在不同客户端之间接力上传。
- 已经上传的文件，再次上传时，可实现快速上传。
- 极少的GC释放。

#### 3.3 Demo示例
 **Demo位置：** [RRQMSocket.FileTransfer.Demo](https://gitee.com/RRQM_Home/RRQMSocket.FileTransfer.Demo)

 **说明：** 可以看到，图一正在上传一个Window的系统镜像文件，大约4.2Gb，传输速度已达到346Mb/s，这是因为服务器和客户端在同一电脑上，磁盘性能限制导致的。其次，GC基本上没有释放，性能非常强悍，图二是下载文件，性能依旧非常强悍。

![上传文件](https://images.gitee.com/uploads/images/2021/0409/190350_92a2ad36_8553710.png "上传文件")
![下载文件](https://images.gitee.com/uploads/images/2021/0409/190954_a212982d_8553710.png "下载文件")
## 四、RPC框架
#### 4.1 创建RPC框架
几行代码就可以搭建出完整的高性能文件传输框架，具体创建步骤详见[RRQMSocket创建RPC高性能微框架，支持任意序列化、out及ref](https://blog.csdn.net/qq_40374647/article/details/109143243?spm=1001.2014.3001.5501)。

#### 4.2 特点
- 简单易用。
- 多线程处理。
- 高性能，在保证送达但不返回的情况下，10w次调用用时0.8s，在返回的情况下，用时3.9s。
- 支持TCP、UDP等不同的协议调用相同服务。
- 支持指定服务异步执行。
- 支持权限管理，让非法调用死在萌芽时期。
- 全自动代码生成，可使用系统编译，也可以自己使用源代码编译。
- 代理方法会生成异步方法，支持客户端异步调用。
- 支持out、ref，参数设定默认值等。
- 随心所欲的序列化方式，除了自带的[超轻量级二进制序列化](https://blog.csdn.net/qq_40374647/article/details/114178244?spm=1001.2014.3001.5501)、xml序列化外，用户可以自己随意使用其他序列化。
- 支持编译式调用，也支持方法名+参数式调用。
- 全异常反馈，服务里发生的异常，会一字不差的反馈到客户端。
- 超简单的回调方式。

#### 4.3 Demo示例
 **Demo位置：** [RRQMSocket.RPC.Demo](https://gitee.com/RRQM_Home/RRQMSocket.RPC.Demo)

 **说明：** 
图一、图二、图三分别为`UDP无反馈调用`、`TCP有反馈调用`、`TCP连接池有反馈调用`。调用次数均为10w次，调用性能非常nice。在无反馈中，吞吐量达14.28w，在有反馈中达2.72w，简直秒杀WCF（WCF使用http协议，在本机测试吞吐量为310）

![输入图片说明](https://images.gitee.com/uploads/images/2021/0409/191343_e5827d04_8553710.png "屏幕截图.png")

![输入图片说明](https://images.gitee.com/uploads/images/2021/0409/191501_abec9e45_8553710.png "屏幕截图.png")

![输入图片说明](https://images.gitee.com/uploads/images/2021/0409/191531_d7f0a8d4_8553710.png "屏幕截图.png")

🤝 如何贡献

## 致谢

谢谢大家对我的支持，如果还有其他问题，请加群QQ：234762506讨论。


## 💕 支持本项目
您的支持就是我不懈努力的动力。打赏时请一定留下您的称呼。

 **赞助总金额:366.6￥** 

**赞助名单：** 

（以下排名只按照打赏时间顺序）

> 1.Bobo Joker

> 2.UnitySir

> 3.Coffee

<img src="https://images.gitee.com/uploads/images/2021/0330/234046_7662fb8c_8553710.png" width = "600" height = "400" alt="图片名称" align=center />


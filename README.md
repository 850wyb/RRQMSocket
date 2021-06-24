<p></p>
<p></p>
<p align="center">
<img src="https://ftp.bmp.ovh/imgs/2021/06/351eeccfadc07014.png" width = "100" height = "100" alt="图片名称" align=center />
</p>

 <div align="center"> 
  
[![NuGet version (RRQMSocket)](https://img.shields.io/nuget/v/RRQMSocket.svg?style=flat-square)](https://www.nuget.org/packages/RRQMSocket/)
[![License](https://img.shields.io/badge/license-Apache%202-4EB1BA.svg)](https://www.apache.org/licenses/LICENSE-2.0.html)
[![Download](https://img.shields.io/nuget/dt/RRQMSocket)](https://www.nuget.org/packages/RRQMSocket/)
[![star](https://gitee.com/dotnetchina/RRQMSocket/badge/star.svg?theme=gvp)](https://gitee.com/dotnetchina/RRQMSocket/stargazers) 
[![fork](https://gitee.com/dotnetchina/RRQMSocket/badge/fork.svg?theme=gvp)](https://gitee.com/dotnetchina/RRQMSocket/members)

</div>  

<div align="center">

合抱之木，生于毫末；九层之台，起于垒土。

</div>
<div align="center">

</div>

## 💿描述
| 名称 |描述|
|---|---|
|[![NuGet version (RRQMSocket)](https://img.shields.io/nuget/v/RRQMSocket.svg?label=RRQMSocket)](https://www.nuget.org/packages/RRQMSocket/)| **RRQMSocket**是一个整合性的、超轻量级的网络通信服务框架。它具有 **高并发连接** 、 **高并发处理** 、 **事件订阅** 、 **插件式扩展** 、 **多线程处理** 、 **内存池** 、 **对象池** 等特点，让使用者能够更加简单的、快速的搭建网络框架。|
| [![NuGet version (RRQMSocket.FileTransfer)](https://img.shields.io/nuget/v/RRQMSocket.FileTransfer.svg?style=flat-square)](https://www.nuget.org/packages/RRQMSocket.FileTransfer/) |  RRQMSocket.FileTransfer是一个高性能的文件传输框架，您可以用它传输**任意大小**的文件，它可以完美支持**上传下载混合式队列传输**、**断点续传**、 **快速上传** 、**传输限速**、**获取文件信息**、**删除文件**等。在实时测试中，它的传输速率可达500Mb/s。 |
|[![NuGet version (RRQMSocket.RPC)](https://img.shields.io/nuget/v/RRQMSocket.RPC.svg?style=flat-square)](https://www.nuget.org/packages/RRQMSocket.RPC/)                            |RRQMSocket.RPC是一个超轻量、高性能、可扩展的微服务管理平台框架，目前已完成开发**RRQMRPC**、**XmlRpc**、**JsonRpc**、**WebApi**部分。**RRQMRPC**部分使用RRQM专属协议，支持客户端**异步调用**，服务端**异步触发**、以及**out**和**ref**关键字，**函数回调**等。在调用效率上也是非常强悍，在调用空载函数，且返回状态时，**10w**次调用仅用时**3.8**秒，不返回状态用时**0.9**秒。其他协议调用性能详看性能评测。|
| [![NuGet version (RRQMSocket.Http)](https://img.shields.io/nuget/v/RRQMSocket.Http.svg?style=flat-square)](https://www.nuget.org/packages/RRQMSocket.Http/)                         |  RRQMSocket.Http是一个能够简单解析Http的服务组件，能够快速响应Http服务请求。|

## 🖥支持环境
- .NET Framework4.5及以上。
- .NET Core3.1及以上。
- .NET Standard2.0及以上。

## 🥪支持框架
- WPF
- Winform
- Blazor
- Xamarin
- Mono
- Unity
- 其他（即所有C#系）

## 🌴RRQMSocket特点速览

#### 对象池

对象池在RRQMSocket有很多应用，最主要的两个就是**连接对象池**和**处理对象池**。连接对象池就是当客户端成功连接时，首先会去连接对象池中找TcpSocketClient，然后没有的话，才会创建。如果哪个客户端掉线了，它的TcpSocketClient就会被回收。这也就是**ID重用**的原因。

然后就是处理对象池，在RRQMSocket中，接收数据的线程和IOCP内核线程是分开的，也就是比如说客户端给服务器发送了1w条数据，但是服务器收到后处理起来很慢，那传统的iocp肯定会放慢接收速率，然后通知客户端的tcp窗口，发生拥塞，然后让客户端暂缓发送。但是在RRQMSocket中会把收到的数据通过队列全都存起来，首先不影响iocp的接收，同时再分配线程去处理收到的报文信息，这样就相当于一个“泄洪湖泊”，能很大程度的提高处理数据的能力。

#### 多线程

由于有**处理对象池**的存在，使多线程处理变得简单。在客户端连接完成时，会自动分配该客户端辅助类（TcpSocketClient）的消息处理逻辑线程，假如服务器线程数量为10，则第一个连接的客户端会被分配到0号线程中，第二个连接将被分配到1号线程中，以此类推，循环分配。当某个客户端收到数据时，会将数据排入当前线程所独自拥有的队列当中，并唤醒线程执行。

#### 传统IOCP和RRQMSocket

RRQMSocket的IOCP和传统也不一样的，以微软官方为例，使用MemoryBuffer开辟一块内存，然后均分，然后给每个会话分配一个区接收，等收到数据以后，再复制一份，然后把复制的数据抛出处理。而RRQMSocket是每次接收之前，从内存池拿一个可用内存块，然后直接用于接收，等收到数据以后，直接就把这个内存块抛出去了，这样就避免了**复制操作**，虽然只是细小的设计，但是在传输**1000w**次**64kb**的数据时，性能相差了**10倍**。所以也是基于此，文件传输时效率才会高。

#### 数据处理适配器

相信大家都使用过其他的Socket产品，例如HPSocket，SuperSocket等，那么RRQMSocket在设计时也是借鉴了其他产品的优秀设计理念，数据处理适配器就是其中之一，但和其他产品的设计不同的是，RRQMSocket的适配器功能更加强大，它可以无视真实的数据，而模拟出想要的数据，例如：可以对数据进行预处理，从而解决数据分包。粘包的问题，也可以直接解析HTTP协议，经过适配器处理后传回一个HttpRequest对象等。

#### 粘包、分包解决

在RRQMSocket中处理TCP粘包、分包问题是非常简单的。只需要更改不同的**数据处理适配器**即可。例如：使用**固定包头**，只需要给TcpSocketClient和TcpClient赋值**FixedHeaderDataHandlingAdapter**的实例即可。同样对应的处理器也有**固定长度** 、 **终止字符分割** 等。

## 🔗联系作者

 - [CSDN博客主页](https://blog.csdn.net/qq_40374647)
 - [哔哩哔哩视频](https://space.bilibili.com/94253567)
 - [源代码仓库主页](https://gitee.com/RRQM_Home) 
 - 交流QQ群：234762506

## ✨API文档

[RRQMSocket API文档](https://gitee.com/RRQM_OS/RRQM/wikis/pages)

 
## 📦 安装

- [Nuget RRQMSocket](https://www.nuget.org/packages/RRQMSocket/)
- [微软Nuget安装教程](https://docs.microsoft.com/zh-cn/nuget/quickstart/install-and-use-a-package-in-visual-studio)

## 🍻RRQM系产品
| 名称| 版本（Nuget Version）|下载（Nuget Download）| 描述 |
|------|----------|-------------|-------|
| [RRQMCore](https://gitee.com/RRQM_OS/RRQMCore) | [![NuGet version (RRQMCore)](https://img.shields.io/nuget/v/RRQMCore.svg?style=flat-square)](https://www.nuget.org/packages/RRQMCore/) | [![Download](https://img.shields.io/nuget/dt/RRQMCore)](https://www.nuget.org/packages/RRQMCore/) | RRQMCore是为RRQM系提供基础服务功能的库，其中包含：**内存池**、**对象池**、**等待逻辑池**、**AppMessenger**、**3DES加密**、**Xml快速存储**、**运行时间测量器**、**文件快捷操作**、**高性能序列化器**、**规范日志接口**等。 |
| [RRQMMVVM](https://gitee.com/RRQM_OS/RRQMMVVM) | [![NuGet version (RRQMMVVM)](https://img.shields.io/nuget/v/RRQMMVVM.svg?style=flat-square)](https://www.nuget.org/packages/RRQMMVVM/) | [![Download](https://img.shields.io/nuget/dt/RRQMMVVM)](https://www.nuget.org/packages/RRQMMVVM/) | RRQMMVVM是超轻简的MVVM框架，但是麻雀虽小，五脏俱全。|
| [RRQMSkin](https://gitee.com/RRQM_OS/RRQMSkin) | [![NuGet version (RRQMSkin)](https://img.shields.io/nuget/v/RRQMSkin.svg?style=flat-square)](https://www.nuget.org/packages/RRQMSkin/) | [![Download](https://img.shields.io/nuget/dt/RRQMSkin)](https://www.nuget.org/packages/RRQMSkin/) | RRQMSkin是WPF的控件样式库，其中包含： **无边框窗体** 、 **圆角窗体** 、 **水波纹按钮** 、 **输入提示筛选框** 、 **控件拖动效果** 、**圆角图片框**、 **弧形文字** 、 **扇形元素** 、 **指针元素** 、 **饼图** 、 **时钟** 、 **速度表盘** 等。|  

## 💐快速入门

## 一、TCP框架
### 1.1 创建服务器


#### 1.4 Demo
[RRQMBox](https://gitee.com/RRQM_OS/RRQMBox)


## 二、文件传输框架

### 2.1 特点

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


#### 2.6 Demo示例

 **Demo位置：** [RRQMBox](https://gitee.com/RRQM_OS/RRQMBox)

 **说明：** 可以看到，图一正在上传一个Window的系统镜像文件，大约4.2Gb，传输速度已达到346Mb/s，这是因为服务器和客户端在同一电脑上，磁盘性能限制导致的。其次，GC基本上没有释放，性能非常强悍，图二是下载文件，性能依旧非常强悍。

![上传文件](https://images.gitee.com/uploads/images/2021/0409/190350_92a2ad36_8553710.png "上传文件")
![下载文件](https://images.gitee.com/uploads/images/2021/0409/190954_a212982d_8553710.png "下载文件")


## 三、RPC框架

RPC框架是所有远程过程调用的微服务管理平台，在该平台的托管下，使多种协议、多种序列化方式调用成为可能。目前可使用RRQMRPC、WebApi、XmlRpc、JsonRpc共同调用。

### 3.1 RRQMRPC

**特点**
- 简单易用。
- 多线程处理。
- 高性能，在保证送达但不返回的情况下，10w次调用用时0.8s，在返回的情况下，用时3.9s。
- 支持TCP、UDP等不同的协议调用相同服务。
- 支持指定服务异步执行。
- 支持权限管理，让非法调用死在萌芽时期。
- 全自动 **代码生成** ，可使用系统编译成dll调用，也可以使用插件生成代理调用。
- 代理方法会生成异步方法，支持客户端异步调用。
- **支持out、ref** ，参数设定默认值等。
- 随心所欲的序列化方式，除了自带的[超轻量级二进制序列化](https://blog.csdn.net/qq_40374647/article/details/114178244?spm=1001.2014.3001.5501)、xml序列化外，用户可以自己随意使用其他序列化。
- 支持编译式调用，也支持方法名+参数式调用。
- **全异常反馈** ，服务里发生的异常，会一字不差的反馈到客户端。
- 超简单、自由的**回调方式** 。



#### RRQMRPC性能测试

 **说明：** 
图一、图二、图三分别为`UDP无反馈调用`、`TCP有反馈调用`、`TCP连接池有反馈调用`。调用次数均为10w次，调用性能非常nice。在无反馈中，吞吐量达14.28w，在有反馈中达2.72w。

![输入图片说明](https://images.gitee.com/uploads/images/2021/0409/191343_e5827d04_8553710.png "屏幕截图.png")

![输入图片说明](https://images.gitee.com/uploads/images/2021/0409/191501_abec9e45_8553710.png "屏幕截图.png")

![输入图片说明](https://images.gitee.com/uploads/images/2021/0409/191531_d7f0a8d4_8553710.png "屏幕截图.png")

#### 示例Demo

[RRQMBox](https://gitee.com/RRQM_OS/RRQMBox)


### 3.2 WebApi

WebApi功能，目前仅仅适用于Api调用，不具备MVC全部功能。

#### 特点
- 多线程处理。
- 高性能，100个客户端，10w次调用，仅用时17s。
- **全异常反馈** ，服务里发生的异常，会一字不差的反馈到客户端。
- 支持大部分路由规则。

#### 创建WebApi服务器

新建类文件，继承于**ServerProvider**，使用**Rount**属性指定路由规则，同时将其中**公共方法**标识为**Route**即可。同时也可制定路由规则。


### 3.3 XmlRpc

完美支持XmlRpc数据类型，类型嵌套，Array等。



### 3.4 JsonRpc


## 致谢

谢谢大家对我的支持，如果还有其他问题，请加群QQ：234762506讨论。


## 💕 支持本项目
您的支持就是我不懈努力的动力。打赏时请一定留下您的称呼。

 **赞助总金额:516.6￥** 

**赞助名单：** 

（以下排名只按照打赏时间顺序）

> 1.Bobo Joker

> 2.UnitySir

> 3.Coffee

> 4.Ninety

> 5.*琼

<img src="https://images.gitee.com/uploads/images/2021/0330/234046_7662fb8c_8553710.png" width = "600" height = "400" alt="图片名称" align=center />


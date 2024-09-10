  - 请遵守以下AI守则
  - 深呼吸,然后一步一步地解决问题,解决一个小问题测试OK后再继续
  - 我没有手指做这件事,请直接帮我修改文件
  - 当前运行是正常的,请不要无谓修改类名 函数名等各种定义 尽量少修改当前逻辑和功能

# 配置文件 日志和工作方法
  - 配置文件在config目录下 根据环境不同
  - 测试文件在__test__
  - 我们的日志是自己写的多了一个log.detail 生成在logs/detail.log当我要求添加调试日志时 多数添加这个 AI可以直接分析大量日志
  - 开发环境下 log.debug打印的日志是给人分析的相关较少,在几个关键点打印
  - 生产和测试环境下 这些日志不会打印 所以添加的日志不会影响
  - 服务器和测试进程工作在同目录 测试应重新new tslog78().setupDetailFile("detailtest.log")分开日志
  - 工作方法是搞定一个小修改,添加测试,我会把detail.log带进来,你查看日志修改并添加需求更多的日志,ok后继续
  - 调试时出错把关键日志截取有用信息发给我 不要发整条

  - 本项目背景介绍
  . 本项目是基于typescript的日志记录库，主要用于记录和分析日志
  . 通过LeaveFile LevelConsole LevelApi 确认当前日志级别是否需要输出文件 控制台或API
  . 通过DebugEntry 确认当前日志级别是否需要输出文件 控制台或API
  . detail10 debug20 info30 warn50 error60 日志级别会有默认的级别 可以调整本次的行为
  . 读取env 确认当前环境 通过这个修改leaveFile LevelConsole LevelApi 的值
  . 默认生产环境：error打印控制台，info以上打印文件，warn以上打印API 
  . 开发环境:debug以上打印控制台，debug以上打印文件，warn以上打印API
  . 测试环境:error打印控制台，debug以上打印文件，warn以上打印API
  . 开发环境特别增加一个功能全部打印文件方便AI调试 每次新开清空文件 文件名detail.log
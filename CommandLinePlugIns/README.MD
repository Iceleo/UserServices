	                             CommandLinePlugIns
	Реализуем поддержку динамических зависимостей. В коде можно использовать классы, 
сборок которых не было на момент компиляции кода. Подключаемые модули смогут иметь разные или даже конфликтующие зависимости.

	Библиотека CommandLinePlugIns\CMDPluginLoadContext. 
	CMDPluginLoadContext "разрешитель" и загрузчик библиотек, выполнена с использованием AssemblyLoadContext.
/// Источник https://github.com/dotnet/samples/blob/master/core/extensions/AppWithPlugin/AppWithPlugin/PluginLoadContext.cs
/// Источник https://docs.microsoft.com/ru-ru/dotnet/core/dependency-loading/understanding-assemblyloadcontext

	Примеры библиотек, которые можно динамичеси загружать:
	- CommandLinePlugIns\helpDinCommandLine 
	- CommandLinePlugIns\parseDinCommandLine
	- CommandLinePlugIns\testDinCommandLine.
Как описываем и используем динамичеси загружаемые библиотеки смотрерть в примере CMDConfigurationTest. 

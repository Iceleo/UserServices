
	Идея разработки.        

	Предположительно это AppControler(контролер приложения), он запускает заданные для запуска 
классы (использую метод Run). Я использую термин - подкоманда, для запукаемого класс. 
AppControler - контролер приложения отрабатывает заявленную загрузку, 
по ситуации, когда нет необходимого класса, и загружает. Заполняет свойства.
	Перед запуском: 
	- считывается необходимые для работы приложения данные, заполняет значения свойств вызываемых 
классов( использую термин подкоманда).
	- проверяет заданный вызов описанным соглашениям о совместимости подкоманд;
вызываемые подкоманды можно динамически подгружать из сборок. Т.е. дорабатываем, и в виде 
задаваемого класса (имеет метод Run, который будет вызван при отработке) заказываем вызов.

	Как мы описываем через:
	- файл конфигурации App.config. 
	- опции командной строки. Опции более значимы и перекрывают значения из файл конфигурации, 
если встречаются и там и там. 
	Если работать только через опции командной строки, необходим шаблон исполнения программы - 
это записанный через вызовы статических методов набор подкоманд и необходимых свойств, которые наполним 
значениями из командной строки. Динамическая подгрузка не работает.
	Если работать используя файл конфигурации, можно динамически подгружать классы из заданных в файле 
конфигурации сборок. Как заполнять Configuration\ConfigurationCMDRun\Readme.md.
Классы(подкоманды) имееют точку входа метод Run. Через этот метод, подключенный код будет вызван.
	В классе можно формировать доп. проверку значений свойств. Она будет вызвана перед вызовом метода Run.
Класс должен иметь конструктор без параметров. 

	Описания в App.config:
	- сборку с путем, порядок вызова,
	- значения свойст, необходимых для работы подкоманды,
	- совместимость работы подкоманд
	Получим конвеер вызова.
	Передача потоков данных между подкомандами пока не продуманна.

и указываем:
	- их обязательность;
	- возможность вызова приложения сразу в нескольких режимах;
	- 
	Обязательные можно проверять на правильность через проверки, оформленные как
спецификации(SpecificationCommandLine). Каждый 


	        Операции из командной строки.
	В шаблоне мы описываем опции командной строки(с др. ветке через конфигурациионные файлы), 
которые нужны программе и их обязательность. 
	Обязательные можно проверять на правильность через проверки, оформленные через спецификации. 
	Это: 
	- проверка достоверности свойства на основе аннотаций данных, 
	- доп. проверки	(с библиотеке: существование заданного имени файла, директории).
	В описании опции командной строки указываем имя класса, который будет вызван с 
заполненными свойствами, указанными в командной строке. Проверки уже выполнены. Совместимость вызова команд
уже проверенна(на основе описанных опции командной строки).


		Строки в командной строке.
Пример задания командной строки (Тест запустить в директории C:\Users\Juryi\Documents\Projects\ShareProj\UserServices\CommandLineTest\).
Корректная строка вызова:
"/parse -pathbase=C:\work\test1\ -pathconfig=C:\work\my_parse -targethost=https://panel.qnits.ru/manager_mvc/offers -depthtransitions=4"
Ошибочная строка вызова: 
"/parse -pathbase=C:\\work\\test1\\ -pathconfig=C:\\work\\my_parse -targethost=https://panel.qnits.ru/manager_mvc/offers -depthtransitions=4 /help -CommandNameHelp=test"
"/parse -pathbase=C:work\\test1\\ -pathconfig=C\\work\\my_parse -targethost=https://panel.qnits.ru/manager_mvc/offers -depthtransitions=4 /help -CommandNameHelp=test"


  <configSections>
    <!--sectionGroup name="customSectionGroups" type="UserServices.ConfigurationCommandLineRun.customSectionGroup"-->
    <!--section name="userSettings"  type="System.Configuration.ClientSettingsSection"/-->
    <!--/sectionGroup-->
	<!--section name="nlog" type="NLog.Config.ConfigSectionHandler, NLog"/  UserServices.-->
     <!--section name="cmdrun"          type="UserServices.ConfigurationCMDRun.ValidatedCMRunSection, ConfigurationCMDRun, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
		allowDefinition="Everywhere" allowExeDefinition="MachineToApplication" />  
     <section name="cmdrun2"          type="ValidatedCMRunSection, ConsoleNewThought, Version=1.0.0.0" />
     <section name="cmdrun3"          type="ValidatedCMRunSection, ConfigurationCMDRun" /-->
     <section name="CmdRunConfs"   type="UserServices.ConfigurationCMDRun.CommandLineRunSection, ConfigurationCMDRun " />
     <section name="cmdrun"        type="UserServices.ConfigurationCMDRun.ValidatedCMRunSection, ConfigurationCMDRun " />  
     <section name="CMDProperties" type="UserServices.ConfigurationCMDRun.CMDPropertySection, ConfigurationCMDRun " />
     <section name="CMDRunSpecifications" type="UserServices.ConfigurationCMDRun.CMDRunSpecificationSection, ConfigurationCMDRun " />

     <!-- allowDefinition="Everywhere" allowExeDefinition="MachineToApplication" restartOnExternalChanges="true" /-->
  </configSections>
      <cmdrun  myTimeSpan="8:15:00" maxCMD="10" maxProp="100" />
      <!-- Секция дополнительных параметров программы  "Подключаемые сборки подкоманд". -->
      <CmdRunConfs>
        <add  subcommand="parseDin" numberExecution="1" assemblyNameWithPath="..\CommandLinePlugIns\parseDinCommandLine\bin\Debug\netcoreapp3.1\parseCommandLine2.dll" description="" />
        <add  subcommand="parse"    numberExecution="4" assemblyNameWithPath="" description="" />
        <add  subcommand="test"     numberExecution="2" assemblyNameWithPath="" description="" />
        <add  subcommand="testDin"  numberExecution="3" assemblyNameWithPath="..\CommandLinePlugIns\testDinCommandLine\bin\Debug\netcoreapp3.1\testCommandLine2.dll" description="" />
      </CmdRunConfs>
     <!-- Секция дополнительных параметров программы  "Свойства и значения подкоманд". -->
      <CMDProperties>
        <add  subcommand="parse" nameProperty="project"          value="C:\work\project1"      description="корневая директория проекта" />
        <add  subcommand="parse" nameProperty="html_files"       value="C:\work\project1\html_files"   description="директория расположения HTML страниц" />
        <add  subcommand="parse" nameProperty="static_files"     value="C:\work\project1\static_files"  description="директория расположения статичных ресурсов" />
        <add  subcommand="parse" nameProperty="depthtransitions" value="3"                     description="Глубина переходов на внутренние страницы" />
        <add  subcommand="test" nameProperty="project"          value="C:\work\project1"      description="корневая директория проекта" />
        <add  subcommand="parseDin" nameProperty="project"          value="C:\work\project1"      description="корневая директория проекта" />
        <add  subcommand="parseDin" nameProperty="html_files"       value="C:\work\project1\html_files"   description="директория расположения HTML страниц" />
        <add  subcommand="parseDin" nameProperty="static_files"     value="C:\work\project1\static_files"  description="директория расположения статичных ресурсов" />
        <add  subcommand="parseDin" nameProperty="depthtransitions" value="3"                     description="Глубина переходов на внутренние страницы" />
        <add  subcommand="testDin" nameProperty="project"          value="C:\work\project1"      description="корневая директория проекта" />

      </CMDProperties>
	 <!-- Секция дополнительных параметров программы  "Подкоманды и спецификации". -->
      <CMDRunSpecifications>
        <add  subcommandLeft="parse" specification="XOR" subcommandRight="parseDin" description="" />
        <add  subcommandLeft="parse" specification="AND" subcommandRight="test"   description=""   />
        <add  subcommandLeft="parse" specification="AND" subcommandRight="testDin"  description=""  />
        <add  subcommandLeft="test"  specification="XOR" subcommandRight="testDin"  description=""  />
      </CMDRunSpecifications>

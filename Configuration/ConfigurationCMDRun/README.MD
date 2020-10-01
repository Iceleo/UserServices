	
	Описание секций, которые используем для разбора из файла конфигурации настройки вызова.
     Разбор конкретного вызова приложения из файла конфигурации.
Используемые секции
     - "CmdRunConfs" секция дополнительных параметров программы "Подкоманды, сборки, порядок вызова".
     - "CMDProperties" секция дополнительных параметров программы  "Свойства и значения подкоманд".
     - "CMDRunSpecifications" секция дополнительных параметров программы "Подкоманды и спецификации".

	Подробней:
	1.CmdRunConfs:	
	- subcommand -имя подкомманды.
	- assemblyNameWithPath - имя динамически загружаемой сборки с путем
	- numberExecution - номер исполнения подкоманды. Если не указан, 
подкоманда не рассматривается в текущем вызове для вызова и исполнения.
	- description - описание назначения подкомманды(необязательно). 

	2. CMDRunSpecifications
	- subcommandLeft - имя подкомманды левой;
	- subcommandRight - имя подкомманды правой;
	- specification - имя спецификации(XOR, AND, OR) совместимости вызова подкоманд.

	3.CMDProperties
	- subcommand - имя подкомманды.
	- nameProperty - имя свойства.
	- value - значение свойства
	- description - описание назначения (необязательно). 
	
 
Для использования установим System.Configuration:
	pm> Install-Package System.Configuration.ConfigurationManager
После этого в .csproj появится
  <ItemGroup>
    <PackageReference Include="System.Configuration.ConfigurationManager" Version="4.7.0" />
  </ItemGroup>


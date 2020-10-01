Нужно установить System.Configuration, для этого:
	pm> Install-Package System.Configuration.ConfigurationManager

После этого в .csproj появится
  <ItemGroup>
    <PackageReference Include="System.Configuration.ConfigurationManager" Version="4.7.0" />
  </ItemGroup>

  <configSections>
     <section name="CmdRunConfs" type="UserServices.ConfigurationCMDRun.CommandLineRunSection, ConfigurationCMDRun " />
  </configSections>

  <CmdRunConfs>
    <add  subcommand="parse" nameProperty="project"          value="C:\work\project1"      description="корневая директория проекта" />
    <add  subcommand="parse" nameProperty="html_files"       value="C:\work\project1\html_files"   description="директория расположения HTML страниц" />
    <add  subcommand="parse" nameProperty="static_files"     value="C:\work\project1\static_files"  description="директория расположения статичных ресурсов" />
    <add  subcommand="parse" nameProperty="depthtransitions" value="3"                     description="Глубина переходов на внутренние страницы" />
    <add  subcommand="test" nameProperty="project"          value="C:\work\project1"      description="корневая директория проекта" />
  </CmdRunConfs>



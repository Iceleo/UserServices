����� ���������� System.Configuration, ��� �����:
	pm> Install-Package System.Configuration.ConfigurationManager

����� ����� � .csproj ��������
  <ItemGroup>
    <PackageReference Include="System.Configuration.ConfigurationManager" Version="4.7.0" />
  </ItemGroup>

  <configSections>
     <section name="CmdRunConfs" type="UserServices.ConfigurationCMDRun.CommandLineRunSection, ConfigurationCMDRun " />
  </configSections>

  <CmdRunConfs>
    <add  subcommand="parse" nameProperty="project"          value="C:\work\project1"      description="�������� ���������� �������" />
    <add  subcommand="parse" nameProperty="html_files"       value="C:\work\project1\html_files"   description="���������� ������������ HTML �������" />
    <add  subcommand="parse" nameProperty="static_files"     value="C:\work\project1\static_files"  description="���������� ������������ ��������� ��������" />
    <add  subcommand="parse" nameProperty="depthtransitions" value="3"                     description="������� ��������� �� ���������� ��������" />
    <add  subcommand="test" nameProperty="project"          value="C:\work\project1"      description="�������� ���������� �������" />
  </CmdRunConfs>



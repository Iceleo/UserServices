	
	�������� ������, ������� ���������� ��� ������� �� ����� ������������ ��������� ������.
     ������ ����������� ������ ���������� �� ����� ������������.
������������ ������
     - "CmdRunConfs" ������ �������������� ���������� ��������� "����������, ������, ������� ������".
     - "CMDProperties" ������ �������������� ���������� ���������  "�������� � �������� ���������".
     - "CMDRunSpecifications" ������ �������������� ���������� ��������� "���������� � ������������".

	���������:
	1.CmdRunConfs:	
	- subcommand -��� �����������.
	- assemblyNameWithPath - ��� ����������� ����������� ������ � �����
	- numberExecution - ����� ���������� ����������. ���� �� ������, 
���������� �� ��������������� � ������� ������ ��� ������ � ����������.
	- description - �������� ���������� �����������(�������������). 

	2. CMDRunSpecifications
	- subcommandLeft - ��� ����������� �����;
	- subcommandRight - ��� ����������� ������;
	- specification - ��� ������������(XOR, AND, OR) ������������� ������ ���������.

	3.CMDProperties
	- subcommand - ��� �����������.
	- nameProperty - ��� ��������.
	- value - �������� ��������
	- description - �������� ���������� (�������������). 
	
 
��� ������������� ��������� System.Configuration:
	pm> Install-Package System.Configuration.ConfigurationManager
����� ����� � .csproj ��������
  <ItemGroup>
    <PackageReference Include="System.Configuration.ConfigurationManager" Version="4.7.0" />
  </ItemGroup>


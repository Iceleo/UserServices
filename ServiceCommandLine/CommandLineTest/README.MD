  <Target Name="PreBuild" BeforeTargets="PreBuildEvent">
    <Exec Command="/parse &#xD;&#xA;-pathbase=C:\work\test1\ &#xD;&#xA;-pathconfig=C:\work\my_parse &#xD;&#xA;-targethost=https://panel.qnits.ru/manager_mvc/offers" />
  </Target>

  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Exec Command="\parse &#xD;&#xA;-pathbase=C:\work\test1\ &#xD;&#xA;-pathconfig=C:\work\my_parse &#xD;&#xA;-targethost=https://panel.qnits.ru/manager_mvc/offers" />
  </Target>


Серьезность	Код	Описание	Проект	Файл	Строка	Состояние подавления
Ошибка	MSB6006	"sgen.exe" завершилась с кодом 1.	SpecificationCommandLine	C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\Microsoft.Common.CurrentVersion.targets	3524	


/parse -pathbase=C:\work\test1\ -pathconfig=C:\work\my_parse -targethost=https://panel.qnits.ru/manager_mvc/offers 
-depthtransitions=4  
/help CommandNameHelp
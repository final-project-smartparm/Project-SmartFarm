# Project-SmartParm
IoT 기술을 활용하여 소형 스마트팜을 구축하는 것을 목표로 합니다. 아두이노 온도센서와 습도센서를 사용하여 환경 데이터를 수집하고, 빅데이터를 이용하여 날씨 데이터베이스를 구축하여 WPF환경에서의 작물 재배 환경을 최적화합니다.  




# SmartFarm-WPF-Part
스마트팜 프로젝트 WPF개발 (UI) 
- 아두이노 보드를 이용하여 식물관리 시스템을 구축하고 , WPF를 사용하여 사용자 인털페이스(UI) 를 개발하여 효율적인 환경 제어와 모니터링을 제공하는 스마트팜을 구현 

## day01
사용기술 
- 소프트웨어 : WPF , XAML , C#

-  WPF를 사용하는 이유
    - XAML 언어로 컨트롤러를 유연하게 디자인 할 수 있기 때문에 스마트팜의 보여지는 UI가 많고 디테일한 시각적인 이미지가 중요하기 때문에 XAML코드 기반으로 사용하면 좋을것이라고 생각
    - 이쁜 디자인 : WPF는 UI를 이쁘게 꾸밀 수 있음 컨트롤러를 자유롭게 배치하고 꾸밀 수 있으면서 애니메이션 효과를 주어 사용자들에게 매력적인 화면을 제공할 수 있어서
    - 디자인과 코드 분리 : 디자인과 behind code 부분을 분리가 가능하기 때문에 서로의 작업이 영향을 미치지 않아서 스마트팜 프로젝트와 적합하다고 생각했음 

- 제어 패널 및 mainWindow UI 만들기
    - MainWindow.xaml
        - 메인 화면 후에 보이는 식물의 정보를 보여주는 메인화면 역할
            - 애플리케이션 로그인 시작 후 식물 종 선택 후 보이는 화면 
            -  전체적인 시스템 상태나 다양한 정보의 통꼐를 한눈에 파악 할 수 있도록 보여줌
                -  물탱크의 급수량 
                -  급수 주기 
                -  온도 
                -  조도센서의 밝기 정도
                -  쿨링팬 작동시간
                -  카메라  
        - HomeControl.xaml
            - 역할 : 메인 대시보드 역할
                - 제어센서 모니터링(PanelControl.xaml) , 실시간상태 모니터링(PanelLiveInfo.xaml) , 성장 진행률 모니터링(PanelPicture.xaml)

                - PanelControl.xaml
                    - 기본 DB값에서 추천해주는 기본값에 맞춰저 있는 각 수치를 사용자의 기준에 맞춰서 조절하고 환경을 최적화 할 수 있는 컨트롤 화면
                    - 물탱크의 급수량, 급수 주기, 온도, 조도 센서의 밝기 정도, 쿨링팬 작동 시간을 제어할 수 있는 기능을 제공
                - ![PanelControl.xaml](https://raw.githubusercontent.com/final-project-smartparm/SmartFarm-WPF-Part/main/img/Sparm1.png)
                - PanelLiveInfo.xaml
                    - 현재 온도 , 토양 수분도 , 조도 , 물탱크 상황 등 스마트팜의 주요 환경 지표를 실시간 반영해여 보여주는 화면
                    - 사용자가 위의 실시간 데이터를 통해서 상태를 파악하고 각 필요한 조치를 취할 수가 있음
                    - ex : 각각의 모니터링화면에서 경고가 발생!
                - ![PanelLiveInfo.xaml](https://raw.githubusercontent.com/final-project-smartparm/SmartFarm-WPF-Part/main/img/Sparm2.png)
                - PanelPicture.xaml
                    - 식물의 성장 과정을 시각적으로 추적 할 수 있는 화면
                    - 주기적으로 촬영된 사진을 앨범처럼 보여주면서 디자인과 성장 상태의 변화를 확인할 수 있음
                - ![PanelPicture.xaml](https://raw.githubusercontent.com/final-project-smartparm/SmartFarm-WPF-Part/main/img/Sparm3.png)




1111
2222
# ADOFAI BPM Normalizer

얼불춤 맵의 타일의 각도에 대한 BPM을 맞춰줌으로서 결과적으로 마법진의 가감속 조절을 자동화 합니다.

이 프로그램을 사용하면 자동으로 무변속 마법진, 패턴변속 마법진(e.g.폴리리듬 법진 ) 등을 생성할 수 있습니다.

### 다운로드
[(windows)](https://github.com/noname0310/AdofaiBpmNormalizer/releases/download/1.2.0/AdofaiBpmNormalizerWin.zip) [(linux)](https://github.com/noname0310/AdofaiBpmNormalizer/releases/download/1.2.0/AdofaiBpmNormalizerLinux.zip) [(mac)](https://github.com/noname0310/AdofaiBpmNormalizer/releases/download/1.2.0/AdofaiBpmNormalizerOsx.app.zip)

### 사용방법

1. 레벨을 에디터에서 ctrl + f 로 변환할 타일의 시작번호와 끝번호를 확인합니다.

![Alt text](/imgsrc/0.png)

2. 프로그램을 실행시킨뒤 Open을 눌러서 레벨을 엽니다 (변환전에 백업을 권장합니다).

![Alt text](/imgsrc/1.png)

3. Start Floor Index 에 시작타일번호를 End Floor Index 에 끝타일 번호를 입력하고 BPM을 입력하여줍니다.
  커스텀패턴은 변속이 존재하는 마법진을 생성할때 사용됩니다. 예를들어 BPM을 150으로 하고 Custom Pattern 에 1, 2, 2, 1 을 넣으시면 타일들의 BPM이 150, 300, 300, 150, 150, 300, 300 ... 이런식으로 반복됩니다. (1, 2, 2, 1 패턴은  폴리리듬입니다.)

  Use BPM Multiplier 모드의 경우에는 가감속이 승수로 작동하며 Use Const BPM Value 모드에선 가감속이 상수로 작동합니다.

![Alt text](/imgsrc/2.png)

4. RUN을 눌렀을때 변환이 성공한다면 좌측 하단에 "성공적으로 정규화가 되었습니다." 라는 메세지가 뜹니다.

![Alt text](/imgsrc/3.png)

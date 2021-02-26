# ADOFAI BPM Normalizer

By aligning the BPM for the tile angle of the ADOFAI map, it  consequently automates the acceleration adjustment of the Magic Circle.

This program allows you to automatically create a non-transmission magic circle, a pattern-transmission magic circle(e.g. polyrhythm), and so on.

얼불춤 맵의 타일의 각도에 대한 BPM을 맞춰줌으로서 결과적으로 마법진의 가감속 조절을 자동화 합니다.

이 프로그램을 사용하면 자동으로 무변속 마법진, 패턴변속 마법진(e.g.폴리리듬 법진 ) 등을 생성할 수 있습니다.

### Downloads (다운로드)
[(windows)](https://github.com/noname0310/AdofaiBpmNormalizer/releases/download/1.0.0/AdofaiBpmNormalizerWin.zip) [(linux)](https://github.com/noname0310/AdofaiBpmNormalizer/releases/download/1.0.0/AdofaiBpmNormalizerLinux.zip) [(mac)](https://github.com/noname0310/AdofaiBpmNormalizer/releases/download/1.0.0/AdofaiBpmNormalizerOsx.app.zip)

### Usage (사용방법)

1. Check the start and end numbers of the tiles to convert the level to ctrl + f in the editor.
레벨을 에디터에서 ctrl + f 로 변환할 타일의 시작번호와 끝번호를 확인합니다.

![Alt text](/imgsrc/0.png)

2. Launch the program and press Open to open the level (recommended backup before conversion).
프로그램을 실행시킨뒤 Open을 눌러서 레벨을 엽니다 (변환전에 백업을 권장합니다).

![Alt text](/imgsrc/1.png)

3. The custom pattern is used to create a magic circle with a pattern-transmission. For example, if you put 150 BPM and 1, 2, 2, 1 in Custom  Pattern, the BPM of the tiles is 150, 300, 300, 150, 150, 300, 300... It repeats like this. (1, 2, 2, 1 pattern is polyrhythm.)
Start Floor Index 에 시작타일번호를 End Floor Index 에 끝타일 번호를 입력하고 BPM을 입력하여줍니다.
커스텀패턴은 변속이 존재하는 마법진을 생성할때 사용됩니다. 예를들어 BPM을 150으로 하고 Custom Pattern 에 1, 2, 2, 1 을 넣으시면 타일들의 BPM이 150, 300, 300, 150, 150, 300, 300 ... 이런식으로 반복됩니다. (1, 2, 2, 1 패턴은  폴리리듬입니다.)

![Alt text](/imgsrc/2.png)

4. If the conversion succeeds when you press RUN, the message "floors bpm has been normalized" appears at the bottom left.
RUN을 눌렀을때 변환이 성공한다면 좌측 하단에 "성공적으로 정규화가 되었습니다." 라는 메세지가 뜹니다.

![Alt text](/imgsrc/3.png)
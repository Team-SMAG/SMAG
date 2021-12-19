# SMAG
AI arithmetic study game
(Game Engine : Unity)
<br><br>

## 환경 설정
(1) 원격 저장소 push 권한 (git clone 후 아래를 입력)
```
> git config credential.helper store
```
<br>

## 기획서
* https://www.notion.so/AI-Arithmetic-Study-Game-Project-878fcf59d3134062bffe50e46dfc75d9
<br><br>
* [간단한 게임 홍보 영상 (Demo)](https://github.com/Team-SMAG/SMAG/blob/master/%EA%B2%8C%EC%9E%84%ED%99%8D%EB%B3%B4%EC%98%81%EC%83%81.mp4)
<br>

## 개발 코드 깃 형상 관리
master branch : pull request 후 merge 로 진행한다. <br>
develop branch : TBD <br>
(이 외에 필요하다면, 별도의 브랜치 생성) <br><br>
항상 git pull 을 통해 로컬을 최신화하고 작업하는 습관을 가집시다. <br><br>

## 클라이언트 및 서버 관련 
__Client__ <br>
게임 플레이 담당 <br>
개발 : Unity, C# <br>
__Server__ <br>
API 제공 및 AI 연산부분 담당 <br>
개발 : AWS, Python (Django)

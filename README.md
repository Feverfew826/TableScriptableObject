# TableScriptableObject
데이터 테이블 유형의 ScriptableObject 제네릭 클래스와 전용 테이블 에디터를 구현한 것입니다.
![Picture of YachuDice](./Images/Image01.PNG)\
MS Excel이나 Google Sheet 등으로 테이블을 작성하여 게임에 도입하는 경우를 많이 봤습니다.\
Unity Editor 내에서 테이블 작업을 수행하고 싶다거나, 파서 작성이나 코드 생성기 작성 등의 복잡함 없이 테이블을 사용하고 싶다는 의도로 이 도구를 작업하기 시작했습니다.

다만, 이 도구는 테이블 크기가 클 때에 쓸만한 도구는 아닙니다. 실험해보니 수백개의 열을 다루는데는 좋지 않았습니다. 아직 세로 스크롤 바도 없구요.\
거대한 프로젝트에는 그 크기에 적합한 솔루션을 사용해야 할 것입니다.

대신 이 도구는 ObjectField를 이용해서 다른 에셋에 대한 참조를 직접 설정할 수 있다는 장점 정도가 있겠네요.\
해보진 않았지만, Addressables 패키지를 사용하고 있다면 AssetReference를 설정할 수도 있을 것 같습니다.

장점:
 - Unity Editor 상에서 작업 가능.
 - 파서 또는 코드 생성기 작성이 필요 없음.
 - 에셋에 대한 참조를 설정할 수 있음.
 - 작은 프로젝트에 적합.

단점:
 - 큰 테이블을 조작할만한 퍼포먼스가 나오지 않음.
 - 세로 스크롤 바가 없음.
 - Inspector 상에서 목록 요소 수 조작 시의 동기화가 불완전함.
 - 테이블 구조 생성 및 수정을 원하면, 코드를 편집해야 함.
 - 큰 프로젝트에 부적합.

최종적으로 작은 프로젝트에서 간단한 테이블을 작성하고 관리하고 싶을 때 사용해볼 법한 패키지입니다.

## 사용 방법
TableScriptableObject<T>의 닫힌 제네릭 클래스를 상속하여, 새로운 테이블 형식을 정의합니다.\
정의 예: `public class ExampleTable : TableScriptableObject<Data>`

유니티 에디터의 상단 메뉴바에서 Window/TableScriptableObject/TableScriptableObjectEditor 메뉴를 통해 에디터를 열 수 있습니다.\
에디터를 연 뒤, 에디터 상단의 ObjectField에 TableScriptableObject 에셋을 설정하는 것으로 테이블처럼 보면서 편집할 수 있습니다.

## ToDo 리스트
- 세로 스크롤 바 추가


## 유의 사항
ScriptableObjectEditor를 연채로 Inspector를 이용해 대상 ScriptableObject 에셋 내 목록의 요소의 수를 늘이거나 줄이면, 이는 ScriptableObjectEditor에 즉시 동기화되지 않습니다.\
ScriptableObjectEditor를 다시 열거나, ScriptableObjectEditor상에서 요소를 추가하면 그제서야 동기화됩니다.\
추후 개선하면 좋겠습니다만, 그렇지 않더라도 작업에 문제는 없어 보여서 개선할 예정은 없습니다.

## Unity6를 사용 중이라면
이 패키지를 사용할 필요가 없습니다.\
[Unity - Manual: MultiColumnListView](https://docs.unity3d.com/6000.0/Documentation/Manual/UIE-uxml-element-MultiColumnListView.html)에 이것보다 훨씬 쉽고 좋은 구현 예가 있습니다.\
2022.3 버전 기준으로 Column 클래스에 bindingPath 필드가 없어서 이렇게 구현할 필요가 있었습니다.


## 샘플
Package Manager에서 샘플 중 'UsageSample'을 임포트하여 테이블 샘플을 확인할 수 있습니다.

## 설치 방법
### 1. 설치
'Package Manager'의 좌상단의 '+' 버튼을 누르고, 'Add package from git URL'을 누르고, 다음 주소를 입력하세요.
`https://github.com/Feverfew826/TableScriptableObject.git?path=Assets/Plugins/TableScriptableObject`

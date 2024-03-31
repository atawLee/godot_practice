## [Signal] 
외부에서 지정할수 있게 열어주는 이벤트 
C# 에서는 delegate에 PickupEventHandler라는 명칭으로 만들고 [Signal] 애트리뷰트를 붙여주면 Pickup 이라는 시그널이 코드제네레이션에 의해서 자동으로 생성됩니다.

## [Export]
godot editor에서 접근가능하도록 열어주는 멤버 항목
</br>
godot editor에서 해당 멤버에 대한 값을 편집 가능해지고, 다른 씬을 대입해놓고 인스턴스로 생성하는 등으로 연계하여 사용할 수 있습니다.

## GetNode<T>("selector")
C#의 셀렉터 gdscript에서는 $selector 형식으로 자식노드를 선택할수 있지만
C#은 GetNode를 이용하면됨.

## Tween 
특정 수학 함수를 사용해 어떤  값을 시간에 따라 보간하는 방법
예제사이트 : https://easings.net 

```C#
var tw = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Quad); // 동시에 트윈을 발생시킴//전환함수를 2차곡선으로 설정
tw.TweenProperty(this, "scale", scale * 3, 0.3);
tw.TweenProperty(this, "modulate:a", 0.0, 0.3);
await ToSignal(tw,"finished"); // await tw.finished
QueueFree(); //개체 삭제
```
트윈에 관한 레퍼런스 문서
[Tween — Godot Engine (stable) documentation in English](https://docs.godotengine.org/en/stable/classes/class_tween.html)


## 리지드 바디 물리학
두 오브젝트가 교차하거나 접촉하는 시점을 파악해야하는 경우, 이를 **콜리전감지**라고 하며, 감지되었을 때 뭔가가 이러나기를 바라는 것을 **콜리전 반응**이라고 합니다.

## 물리바디 
고도는 3가지 종류의 물리바드를 제공, 이는 PhisicsBody2D 노드 유형으로 그룹화 되어있습니다.

### StaticBody2D
물리 엔진의 영향을 받지 않음(벽,지면 등)
### RigidBody2D
리지드바디는 물리시뮬레이션을 제공하는 바디. 물리 바디의 위치를 프로그래머가 직접 제어하지 않아도 됨. 내장된 물리엔진에 따라서 효과를 계산하여 표현합 니다.

### CharacerBody2D
콜리전 감지는 제공하지만 반응은 직접 구현해야함.

## 프로젝트 설정 - 고급 설정 
기본 선형 댐핑 - 전진속도가 얼마나 빨리 줄어들지 제어합니다.
기본 각도 댐핑 - 회전속도가 얼마나 빨리 줄어들지 제어합니다.

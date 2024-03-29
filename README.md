## Signal 
외부에서 지정할수 있게 열어주는 이벤트 

## Export 
godot editor에서 접근가능하도록 열어주는 멤버 항목
</br>
편하게 다른 씬을 끌어다 쓸수 있음.

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



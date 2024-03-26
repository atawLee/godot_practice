using Godot;
using System;
using System.Threading.Tasks;

public partial class hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		
	}

	public void UpdateScore(int value)
	{
		GetNode<Label>("MarginContainer/Score").Text = value.ToString();
	}

	public void UpdateTime(int value)
	{
		GetNode<Label>("MarginContainer/Time").Text = value.ToString();
	}

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		GetNode<Timer>("Timer").Start();
	}

	private void _on_timer_timeout()
	{
		var message = GetNode<Label>("Message");
		message.Hide();
	}
	
	private void _on_start_button_pressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("Message").Hide();
		EmitSignal(SignalName.StartGame);
	}

	private async Task ShowGameOver()
	{
		var timer = GetNode<Timer>("Timer");
		ShowMessage("Game Over");
		await ToSignal(timer, "timeout"); // 타이머의 timeout 시그널이 발생할 때까지 기다립니다.

		GetNode<Button>("StartButton").Show();
		var message = GetNode<Label>("Message");
		message.Text = "Coin Dash !";
		message.Show();
	}
}
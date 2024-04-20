using Godot;
using System;
using Godot.Collections;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartEventHandler();

	public Array<Node> LivesCounter =>GetNode("MarginContainer/HBoxContainer/LiveCounter").GetChildren();
	public Label ScoreLabel => GetNode<Label>("MarginContainer/HBoxContainer/ScoreLabel");
	public Label Message => GetNode<Label>("VBoxContainer/Message");

	public TextureButton StartButton => GetNode<TextureButton>("VBoxContainer/StartButton");

	private bool isGameOver = false;
   
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void ShowMessage(string text)
	{
		Message.Text = text;
		Message.Show();
		
		GetNode<Timer>("Timer").Start();
	}

	public void UpdateScore(int value)
	{
		ScoreLabel.Text = value.ToString();
	}

	public void UpdateLives(int value)
	{
		for (int i = 0; i < 3; i++)
		{
			(LivesCounter[i] as TextureRect).Visible = value > i;
		}
	}

	public void GameOver()
	{
		isGameOver = true;
		ShowMessage("Game Over");
		
		
	}
	
	private void _on_timer_timeout()
	{
		Message.Hide();
		Message.Text = ""; 
		if (isGameOver)
		{
			StartButton.Show();	
			isGameOver = false;
		}
		
	}
	
	private void _on_start_button_pressed()
	{
		StartButton.Hide();
		EmitSignal(SignalName.Start);
	}
	
	

}






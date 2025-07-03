using Godot;
using System;
using static Godot.TextServer;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 200f;
	[Export]
	public float JumpVelocity = 400f;
	private Vector2 _velocity = Vector2.Zero;

	AnimationTree _animationTree;

	public override void _Ready()
	{
		// Initialization code can go here if needed
		_animationTree = GetNode<AnimationTree>("AnimationTree");

		_animationTree.Active = true;
		_animationTree.Set("parameters/conditions/move", true);
	}

	public override void _PhysicsProcess(double delta)
	{
		_velocity.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		_velocity.X *= Speed;
		if (IsOnFloor())
		{
			_animationTree.Set("parameters/conditions/move", true);
			_animationTree.Set("parameters/conditions/jump", false);
			_animationTree.Set("parameters/conditions/dodge", false);

			if (Input.IsActionJustPressed("jump"))
			{
				_velocity.Y = -JumpVelocity;
				_animationTree.Set("parameters/conditions/move", false);
				_animationTree.Set("parameters/conditions/jump", true);
			}

			if (Input.IsActionPressed("dodge"))
			{
				var direction = _velocity.X;
				_velocity.X = 0f;
				_velocity.Y = 0f;
				_animationTree.Set("parameters/conditions/move", false);
				_animationTree.Set("parameters/conditions/jump", false);
				_animationTree.Set("parameters/conditions/dodge", true);
				_animationTree.Set("parameters/Dodge/blend_position", direction);
			}
		}
		else
		{
			_velocity.Y += GetGravity().Y * (float)delta; // Gravity
			_animationTree.Set("parameters/Jump/blend_position", _velocity.X);
		}

		Velocity = _velocity;

		_animationTree.Set("parameters/Move/blend_position", _velocity.X);

		MoveAndSlide();
	}
}

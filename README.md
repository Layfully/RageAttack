# RageAttack
This game was made using unity3d engine.

This is local multiplayer game in which 2 players face off in battle.
Every player controls one of 3 character which he chooses at the beginning.
Every character has special suprpower.
Player can move around, jump normally, charge jump to jump higher, flash some fixed distance ( this action consumes some chi), kick, use normal ability and use superpower.
Also there are some objects scattered around maps which player can pick up and throw them at the enemy to deal damage.
Every player has healthbar.
Every player has chi bar - chi is rechargable energy used to cast superpowers, special abilities and flash. It can be recharged by holding recharge key.
After the match statisctics are shown.

The most unique feature of this game is destructible environment.
It is implemented by clearing pixels of bitmap and recalucating collider. It is slow and complex operation so I decided to give a little bit of time to merge 
multiple desctructions of terrain into one which is applied after this time.

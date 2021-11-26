main()
	var int _teste1 = 3, _teste2 = 2;
	var int _teste3;

	if _teste1 > _teste2 then
		_teste3 = 10;
	end;

	if _teste2 > _teste1 then
		_teste3 = 25;
	else
		_teste3 = 1234;
	end;

	float _teste_float;

	scan(_teste_float);

	var int _i = 0;

	while _i < 10 do 
		print("teste de looping");
	loop;
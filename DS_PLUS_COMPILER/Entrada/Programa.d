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

	var float _teste_float;

	scan(_teste_float);

	var intu _i = 0;

	while _i < 10 do 
		var int _int_local = 34, _int_local2 = 20;

		var int _int3 = _int_local + _int_local2;

		print("teste de looping");
	loop;
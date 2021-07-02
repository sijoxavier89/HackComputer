// Bootstrap code
@256
D=A
@0
M=D

//call Sys.init 0
// push retAddrLabel
@Sys.init$ret.0
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@0
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Sys.init
0;JMP
(Sys.init$ret.0)

//Sys.init 0
(Sys.init)

@0
D=A
@SP
M=D+M

// C_PUSH constant 4000
@4000
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP pointer 0
@SP
M=M-1
A=M
D=M

@THIS
M=D


// C_PUSH constant 5000
@5000
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP pointer 1
@SP
M=M-1
A=M
D=M

@THAT
M=D


//call Sys.main 0
// push retAddrLabel
@Sys.main$ret.1
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@0
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Sys.main
0;JMP
(Sys.main$ret.1)

// C_POP temp 1
@SP
M=M-1
A=M
D=M

@6
M=D


(Sys.init.LOOP)
@Sys.init.LOOP
0;JMP

//Sys.main 5
(Sys.main)
// initialize locals
@LCL
D=M
@0
D=D+A
@R13
M=D
@0
D=A
@R13
A=M
M=D
@LCL
D=M
@1
D=D+A
@R13
M=D
@0
D=A
@R13
A=M
M=D
@LCL
D=M
@2
D=D+A
@R13
M=D
@0
D=A
@R13
A=M
M=D
@LCL
D=M
@3
D=D+A
@R13
M=D
@0
D=A
@R13
A=M
M=D
@LCL
D=M
@4
D=D+A
@R13
M=D
@0
D=A
@R13
A=M
M=D

@5
D=A
@SP
M=D+M

// C_PUSH constant 4001
@4001
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP pointer 0
@SP
M=M-1
A=M
D=M

@THIS
M=D


// C_PUSH constant 5001
@5001
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP pointer 1
@SP
M=M-1
A=M
D=M

@THAT
M=D


// C_PUSH constant 200
@200
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP local 1
@LCL
D=M
@1
D=D+A
@R13
M=D
@SP
M=M-1
A=M
D=M

@R13
A=M
M=D


// C_PUSH constant 40
@40
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP local 2
@LCL
D=M
@2
D=D+A
@R13
M=D
@SP
M=M-1
A=M
D=M

@R13
A=M
M=D


// C_PUSH constant 6
@6
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP local 3
@LCL
D=M
@3
D=D+A
@R13
M=D
@SP
M=M-1
A=M
D=M

@R13
A=M
M=D


// C_PUSH constant 123
@123
D=A

@SP
A=M
M=D
@SP
M=M+1


//call Sys.add12 1
// push retAddrLabel
@Sys.add12$ret.2
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@1
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Sys.add12
0;JMP
(Sys.add12$ret.2)

// C_POP temp 0
@SP
M=M-1
A=M
D=M

@5
M=D


// C_PUSH local 0
@LCL
D=M
@0
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH local 1
@LCL
D=M
@1
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH local 2
@LCL
D=M
@2
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH local 3
@LCL
D=M
@3
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH local 4
@LCL
D=M
@4
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// add
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=D+M
@SP
A=M
M=D
@SP
M=M+1


// add
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=D+M
@SP
A=M
M=D
@SP
M=M+1


// add
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=D+M
@SP
A=M
M=D
@SP
M=M+1


// add
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=D+M
@SP
A=M
M=D
@SP
M=M+1


// return
@LCL
D=M
@R14
M=D
// retAddr = *(endFrame-5)
@5
D=A
@LCL
D=M-D
A=D
D=M
@R13
M=D
// *ARG=pop()
@SP
M=M-1
A=M
D=M

@ARG
A=M
M=D
// SP=ARG+1
@ARG
D=M
@1
D=D+A
@SP
M=D
//THAT = *(endFrame-1)
@R14
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
//THIS = *(endFrame-2)
@R14
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
//ARG = *(endFrame-3)
@R14
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
//LCL = *(endFrame-4)
@R14
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
// goto retAddr
@R13
A=M
0;JMP
@SP
M=M-1
A=M
D=M


//Sys.add12 0
(Sys.add12)

@0
D=A
@SP
M=D+M

// C_PUSH constant 4002
@4002
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP pointer 0
@SP
M=M-1
A=M
D=M

@THIS
M=D


// C_PUSH constant 5002
@5002
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP pointer 1
@SP
M=M-1
A=M
D=M

@THAT
M=D


// C_PUSH argument 0
@ARG
D=M
@0
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 12
@12
D=A

@SP
A=M
M=D
@SP
M=M+1


// add
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=D+M
@SP
A=M
M=D
@SP
M=M+1


// return
@LCL
D=M
@R14
M=D
// retAddr = *(endFrame-5)
@5
D=A
@LCL
D=M-D
A=D
D=M
@R13
M=D
// *ARG=pop()
@SP
M=M-1
A=M
D=M

@ARG
A=M
M=D
// SP=ARG+1
@ARG
D=M
@1
D=D+A
@SP
M=D
//THAT = *(endFrame-1)
@R14
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
//THIS = *(endFrame-2)
@R14
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
//ARG = *(endFrame-3)
@R14
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
//LCL = *(endFrame-4)
@R14
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
// goto retAddr
@R13
A=M
0;JMP
@SP
M=M-1
A=M
D=M


(END)
@END
0;JMP


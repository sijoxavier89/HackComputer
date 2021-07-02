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

//Main.fibonacci 0
(Main.fibonacci)

@0
D=A
@SP
M=D+M

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


// C_PUSH constant 2
@2
D=A

@SP
A=M
M=D
@SP
M=M+1


// lt
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@true0
D;JLT
D=0
@push0
0;JMP
(true0)
D=-1
(push0)
@SP
A=M
M=D
@SP
M=M+1


@SP
M=M-1
A=M
D=M
@Main.fibonacci.IF_TRUE
D;JNE

@Main.fibonacci.IF_FALSE
0;JMP

(Main.fibonacci.IF_TRUE)
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


(Main.fibonacci.IF_FALSE)
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


// C_PUSH constant 2
@2
D=A

@SP
A=M
M=D
@SP
M=M+1


// sub
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@SP
A=M
M=D
@SP
M=M+1


//call Main.fibonacci 1
// push retAddrLabel
@Main.fibonacci$ret.1
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
@Main.fibonacci
0;JMP
(Main.fibonacci$ret.1)

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


// C_PUSH constant 1
@1
D=A

@SP
A=M
M=D
@SP
M=M+1


// sub
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@SP
A=M
M=D
@SP
M=M+1


//call Main.fibonacci 1
// push retAddrLabel
@Main.fibonacci$ret.2
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
@Main.fibonacci
0;JMP
(Main.fibonacci$ret.2)

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


//Sys.init 0
(Sys.init)

@0
D=A
@SP
M=D+M

// C_PUSH constant 4
@4
D=A

@SP
A=M
M=D
@SP
M=M+1


//call Main.fibonacci 1
// push retAddrLabel
@Main.fibonacci$ret.3
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
@Main.fibonacci
0;JMP
(Main.fibonacci$ret.3)

(Sys.init.WHILE)
@Sys.init.WHILE
0;JMP

(END)
@END
0;JMP


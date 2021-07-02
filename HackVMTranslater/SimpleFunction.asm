//SimpleFunction.test 2
(SimpleFunction.test)
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

@2
D=A
@SP
M=D+M

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


// not
@SP
M=M-1
A=M
M=!M
@SP
M=M+1


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


// C_PUSH argument 1
@ARG
D=M
@1
A=D+A
D=M

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


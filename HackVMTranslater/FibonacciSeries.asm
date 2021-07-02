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


// C_POP pointer 1
@SP
M=M-1
A=M
D=M

@THAT
M=D


// C_PUSH constant 0
@0
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP that 0
@THAT
D=M
@0
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


// C_PUSH constant 1
@1
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_POP that 1
@THAT
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


// C_POP argument 0
@ARG
D=M
@0
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


(MAIN_LOOP_START)
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


@SP
M=M-1
A=M
D=M
@COMPUTE_ELEMENT
D;JNE

@END_PROGRAM
0;JMP

(COMPUTE_ELEMENT)
// C_PUSH that 0
@THAT
D=M
@0
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH that 1
@THAT
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


// C_POP that 2
@THAT
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


// C_PUSH pointer 1
@THAT
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


// C_POP argument 0
@ARG
D=M
@0
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


@MAIN_LOOP_START
0;JMP

(END_PROGRAM)
(END)
@END
0;JMP


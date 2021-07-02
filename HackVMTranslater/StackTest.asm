// C_PUSH constant 17
@17
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 17
@17
D=A

@SP
A=M
M=D
@SP
M=M+1


// eq
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@true0
D;JEQ
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


// C_PUSH constant 17
@17
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 16
@16
D=A

@SP
A=M
M=D
@SP
M=M+1


// eq
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@true1
D;JEQ
D=0
@push1
0;JMP
(true1)
D=-1
(push1)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 16
@16
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 17
@17
D=A

@SP
A=M
M=D
@SP
M=M+1


// eq
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@true2
D;JEQ
D=0
@push2
0;JMP
(true2)
D=-1
(push2)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 892
@892
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 891
@891
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
@true3
D;JLT
D=0
@push3
0;JMP
(true3)
D=-1
(push3)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 891
@891
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 892
@892
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
@true4
D;JLT
D=0
@push4
0;JMP
(true4)
D=-1
(push4)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 891
@891
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 891
@891
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
@true5
D;JLT
D=0
@push5
0;JMP
(true5)
D=-1
(push5)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 32767
@32767
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 32766
@32766
D=A

@SP
A=M
M=D
@SP
M=M+1


// gt
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@true6
D;JGT
D=0
@push6
0;JMP
(true6)
D=-1
(push6)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 32766
@32766
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 32767
@32767
D=A

@SP
A=M
M=D
@SP
M=M+1


// gt
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@true7
D;JGT
D=0
@push7
0;JMP
(true7)
D=-1
(push7)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 32766
@32766
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 32766
@32766
D=A

@SP
A=M
M=D
@SP
M=M+1


// gt
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@true8
D;JGT
D=0
@push8
0;JMP
(true8)
D=-1
(push8)
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 57
@57
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 31
@31
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 53
@53
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


// C_PUSH constant 112
@112
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


// neg
@SP
M=M-1
A=M
M=-M
@SP
M=M+1


// and
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=D&M
@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 82
@82
D=A

@SP
A=M
M=D
@SP
M=M+1


// or
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=D|M
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


(END)
@END
0;JMP


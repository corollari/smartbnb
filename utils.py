def leftpad(a, n):
    return "0"*(n-len(a))+a

def num2byteArray(num):
    hexed = hex(num)[2:]
    if(hexed[-1]=="L"):
        hexed=hexed[:-1]
    hexed=leftpad(hexed, 64)
    hexed = [hexed[i:i+2] for i in range(0, len(hexed), 2)]
    hexed.reverse()
    return "{0x"+", 0x".join(hexed)+"}"

def reverseNum(n):
    h = hex(n)
    h = "0"+h if (len(hex)%2==1) else h
    hr = [h[i: i+2] for i in range(0, len(h), 2)]
    hr.reverse()
    return "".join(hr)

p = 2**255 - 19
print("p = ", num2byteArray(p))

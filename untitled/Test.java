public class Test {
    int size;
    //파일인 경우
    public int getSize() {
        return this.size;
    }

    //서브폴더인 경우
    public int getSize(int a) {
        int tmp = 0;
        for all f in folders {
            tmp += f.getSize();
        }
        return tmp;
    }
}

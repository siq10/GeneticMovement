i=0
cd "D:"
cd "Info/Builds"
while [ $i -le 10 ]
do
echo "Running GenLearn${i}"
./GenLearn.exe  -batchmode -nographics > "../Logs/log$i.txt" &
i=$(( i+1 ))
done

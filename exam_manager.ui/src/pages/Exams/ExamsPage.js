import "./Exams.css";
import { useEffect, useState } from "react";
import examService from "../../services/examService";

export default function Exams() {
  const [studentExams, setStudentExams] = useState([]);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    const result = await examService.getStudentExams(
      Number(sessionStorage.getItem("userId"))
    );
    setStudentExams(result.data);
  };

  return (
    <>
      {studentExams.length > 0 ? (
        studentExams.map((exam) => {
          return (
            <ul>
              <li>{exam.SubjectName}</li>
              <li>Datum roka: {exam.DeadlineDate}</li>
              <li>Prijave do: {exam.ApplicationsDate}</li>
              <li>Odjave do: {exam.CheckOutDate}</li>
            </ul>
          );
        })
      ) : (
        <h1>No exams</h1>
      )}
    </>
  );
}

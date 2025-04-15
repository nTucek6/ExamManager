import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { format } from "date-fns";

import Select from "react-select";
import { BarLoader } from "react-spinners";

import examService from "../../../services/examService";

import "./CreateExam.css";

export default function CreateExam() {
  const { id } = useParams();

  const [subject, setSubject] = useState();
  const [location, setLocation] = useState("");
  const [examDate, setExamDate] = useState("");

  const [subjects, setSubjects] = useState([]);

  const [loading, setLoading] = useState(true);

  const fetchSubject = async () => {
    const result = await examService.getProfesorSubjects(
      Number(sessionStorage.getItem("userId"))
    );
    await setSubjects(result.data);
    setLoading(false);
  };

  const fetchExamForUpdate = async () => {
    const result = await examService.getProfessorExamPeriod(id);

    const data = result.data;

    setSubject(subjects.find((find) => find.value === data.SubjectId));
    setLocation(data.ExamLocation);
    setExamDate(format(new Date(data.DeadlineDate), "yyyy-MM-dd"));
    setLoading(false);
  };

  useEffect(() => {
    fetchSubject();
  }, []);

  useEffect(() => {
    if (subjects.length > 0 && id) {
      fetchExamForUpdate();
    } else {
      clearForm();
    }
  }, [subjects, id]);

  const handleSubmit = (e) => {
    e.preventDefault();

    const examData = {
      ExamId: id,
      SubjectId: subject.value,
      ProfessorId: Number(sessionStorage.getItem("userId")),
      ExamLocation: location.trim(),
      DeadlineDate: examDate,
    };

    if (id) {
      examService.updateExam(examData);
    } else {
      examService.createExamPeriod(examData);
    }
    clearForm();
  };

  const handleChange = (option) => {
    setSubject(option);
  };

  const clearForm = () => {
    setLocation("");
    setSubject();
    setExamDate("");
  };

  return (
    <div className="custom-wrapper">
      <form className="custom-form" onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Odaberite predmet</label>
          <Select
            options={subjects}
            value={subject}
            onChange={handleChange}
            placeholder="Odaberite predmet:"
          />
        </div>
        <div className="form-group">
          <label>Upišite lokaciju</label>
          <input
            type="text"
            value={location || ""}
            onChange={(d) => setLocation(d.target.value)}
            className="form-input"
          />
        </div>
        <div className="form-group">
          <label>Unesite datum i vrijeme ispita</label>
          <input
            type="datetime-local"
            value={examDate || ""}
            onChange={(d) => setExamDate(d.target.value)}
            className="form-input"
          />
        </div>

        {loading ? (
          <BarLoader />
        ) : id ? (
          <button type="submit" className="form-button">
            Ažuriraj ispitni rok
          </button>
        ) : (
          <button type="submit" className="form-button">
            Dodaj ispitni rok
          </button>
        )}
      </form>
    </div>
  );
}

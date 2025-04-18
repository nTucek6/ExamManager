import React, { useEffect, useState } from "react";
import { Calendar, dateFnsLocalizer } from "react-big-calendar";
import {
  format,
  parse,
  startOfWeek,
  getDay,
  addMonths,
  subMonths,
} from "date-fns";
import "react-big-calendar/lib/css/react-big-calendar.css";
import studentExamService from "../services/studentExamService";
import examService from "../services/examService";
import "./Scheduler.css";

// Ovdje mozemo na hrv postaviti ali neka bude na EN za sada
const locales = {
  "en-US": require("date-fns/locale/en-US"),
};

const localizer = dateFnsLocalizer({
  format,
  parse,
  startOfWeek,
  getDay,
  locales,
});

export default function Scheduler() {
  const [events, setEvents] = useState([]);
  const [view, setView] = useState("month");
  const [currentDate, setCurrentDate] = useState(new Date());

  const fetchData = async () => {
    const role = sessionStorage.getItem("role");
    const userId = Number(sessionStorage.getItem("userId"));

    if (role === "Student") {
      return await studentExamService.getStudentExams(userId);
    } else if (role === "Professor") {
      return await examService.getProfesorExams(userId);
    }

    return [];
  };

  const fetchEvents = async () => {
    try {
      const result = await fetchData();

      const mappedEvents = result.data.map((exam) => ({
        title: exam.SubjectName,
        start: new Date(exam.DeadlineDate),
        end: new Date(new Date(exam.DeadlineDate).getTime() + 60 * 60 * 1000),
        allDay: false,
      }));
      setEvents(mappedEvents);
    } catch (err) {
      console.error("Greška pri dohvaćanju događaja:", err);
    }
  };

  useEffect(() => {
    fetchEvents();
  }, []);

  const handleViewChange = (newView) => {
    setView(newView);
  };

  const handleNext = () => {
    const newDate = addMonths(currentDate, 1);
    setCurrentDate(newDate);
  };

  const handlePrevious = () => {
    const newDate = subMonths(currentDate, 1);
    setCurrentDate(newDate);
  };

  const handleCurrentMonth = () => {
    setCurrentDate(new Date());
  };

  const currentMonthName = format(currentDate, "MMMM yyyy");

  return (
    <div className="scheduler-container">
      <h2>Raspored ispita</h2>

      <div className="month-display">
        <h3>{currentMonthName}</h3>
      </div>

      <div className="view-buttons">
        <button onClick={() => handleViewChange("day")}>Day</button>
        <button onClick={() => handleViewChange("week")}>Week</button>
        <button onClick={() => handleViewChange("month")}>Month</button>
        <button onClick={() => handleViewChange("agenda")}>Agenda</button>
      </div>

      <div className="navigation-buttons">
        <button onClick={handlePrevious}>&lt; Previous</button>
        <button onClick={handleNext}>Next &gt;</button>
        <button onClick={handleCurrentMonth}>Current Month</button>
      </div>

      <Calendar
        localizer={localizer}
        events={events}
        startAccessor="start"
        endAccessor="end"
        views={["month", "week", "day", "agenda"]}
        view={view}
        defaultView="month"
        toolbar={false}
        style={{ height: 500 }}
        date={currentDate}
      />
    </div>
  );
}

import "./SearchBar.css"

export default function SearchBar({ handleSearch }) {
  return (<div className="search-container">
<input
      type="text"
      className="search-field"
      placeholder="Pretraži po nazivu predmeta:"
      onChange={(input) => handleSearch(input.target.value)}
    />
  </div>
    
  );
}

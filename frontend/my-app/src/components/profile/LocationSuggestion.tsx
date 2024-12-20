import React, {useState} from 'react';
import "../../styles/form.css";

interface Suggestion {
    place_id: string;
    display_name: string;
    type: string
}

interface LocationSuggestionProps {
    onSelectLocation : (city: string, country: string) => void  
}

export default function LocationSuggestion(props : LocationSuggestionProps) {
    const [query, setQuery] = useState('');
    const [suggestions, setSuggestions] = useState<Suggestion[]>([]);
    const [showDropdown, setShowDropdown] = useState(false);

    const handleInputChange = async (e : React.ChangeEvent<HTMLInputElement>) => {
        setQuery(e.target.value);
        if (e.target.value.length > 2) {
            const response = await fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${e.target.value}`);
            const data : Suggestion[] = await response.json();
            const citySuggestions = data.filter((item: Suggestion) =>
                ["city", "administrative"].includes(item.type)
            );
            const formattedSuggestions = citySuggestions.map((item) => {
                const parts = item.display_name.split(', ');
                const city = parts[0];
                const country = parts[parts.length - 1];
                return { ...item, display_name: `${city}, ${country}` };
            });
            setSuggestions(formattedSuggestions);
            setShowDropdown(true);
           
        } else {
            setSuggestions([]);
            setShowDropdown(false);
        }
    };

    const handleSuggestionClick = (suggestion: Suggestion) => {
        const parts = suggestion.display_name.split(', ');
        const city = parts[0];
        const country = parts[parts.length - 1];
        
        props.onSelectLocation(city, country);
        
        setQuery(suggestion.display_name);
        setShowDropdown(false);
    };

    return(
        <div className="location">
            <label htmlFor='location-input'>Set your location</label>
            <input
                id='location-input'
                className="location-input"
                type="text"
                value={query}
                onChange={handleInputChange}
                placeholder="Search location"
                required
            />
            {showDropdown && suggestions.length > 0 && (
                <ul className="location-dropbox">
                    {suggestions.map((suggestion) => (
                        <li
                            className='location-suggestion'
                            key={suggestion.place_id}
                            onClick={() => handleSuggestionClick(suggestion)}
                        
                        >
                            {suggestion.display_name}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
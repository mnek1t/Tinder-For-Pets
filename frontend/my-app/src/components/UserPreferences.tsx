import "../styles/profile.css"
import "../styles/profile.css"
export interface UserPreferencesProps {
    distance: number;
    onDistanceChange: (value: number) => void;
    showPetsInRange: boolean;
    onToggleShowPets: () => void;
    onLightMode: () => void;
    onDarkMode: () => void;
    handleLogout: (event : React.MouseEvent<HTMLButtonElement>) => void;
    handleDeleteAccount: (event : React.MouseEvent<HTMLButtonElement>) => void;
  }

export default function UserPreferences(props : UserPreferencesProps) {
    return(
        <>
            <h3 className="sidebar-title">ACCOUNT SETTINGS</h3>
            <button className="preference-btn">Manage Payment Account</button>
            <h3 className="sidebar-title">DISCOVERY SETTINGS</h3>
            <div>
                <div className="distance-info">
                    <label htmlFor="distance-slider">Distance Preference</label>
                    <label htmlFor="distance-slider">13km {props.distance}</label>
                </div>
                <input id="distance-slider" type="range" min="1" max="100" value="50" onChange={(e) => props.onDistanceChange(Number(e.target.value))}/>

                <div className="toggler-preference">
                    <label htmlFor="toggler">Only show pets in this range</label>
                    <label className="switch">
                        <input className="toggler" type="checkbox" id="toggler" checked={props.showPetsInRange} onChange={props.onToggleShowPets}/>
                        <span className="slider round"></span>
                    </label>
                </div>    
            </div>
            
            <h3 className="sidebar-title">APPEARANCE</h3>
            <button className="preference-btn" onClick={props.onLightMode}>Light Mode</button>
            <button className="preference-btn" onClick={props.onDarkMode}>DarkMode</button>

            <h3 className="sidebar-title">LEGAL</h3>
            
            <div className="legal-section">
                <a href="#Privacy Settings">Privacy Settings</a>
                <a href="#Cookie Policy">Cookie Policy</a>
                <a href="#Privacy Policy">Privacy Policy</a>
                <a href="#Terms of Service">Terms of Service</a>
            </div>
            <br/>
            <button className="preference-btn" onClick={props.handleLogout}>Log out</button>
            <button className="preference-btn" onClick={props.handleDeleteAccount}>Delete Account</button>
        </>
    );
}
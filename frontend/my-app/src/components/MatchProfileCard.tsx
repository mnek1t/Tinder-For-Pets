import "../styles/match-profile-card.css"
interface MatchDataProps {
    matchData: {
        id: string;
        name: string;
        imageData: string;
        imageFormat: string;
        age: number;
        description: string;
        isVaccinated: boolean;
        isSterilized: boolean
    };
}
export default function MessageProfileCard({matchData} : MatchDataProps) {
    return(
    <div className="match-profile">
        <h3 className="match-profile__name">{matchData.name}, {matchData.age}</h3>
        <img className="match-proifle__image" src={`data:${matchData.imageFormat};base64,${matchData.imageData}`} alt="match-profile"></img>
        <div className="box">
            <h4 className="match-profile__description">Description</h4>
            <p className="match-profile__description">{matchData.description}</p>
        </div>
        
        <div className="match-profile__medicine-info box">
            <span className="custom-checkbox">
                <label>
                    isVacinated
                </label>
                <input type="checkbox" checked={matchData.isVaccinated} disabled/>
                    <span className="checkmark" style={{display: 'flex'}}></span>
            </span>
            <span className="custom-checkbox">
                <label>
                    isSterilized
                </label>
                <input type="checkbox" checked={matchData.isSterilized} disabled/>
                <span className="checkmark" style={{display: 'flex'}}></span>
                
            </span>
        </div>
        <br/>
        <button className="preference-btn" >Umatch</button>
        <button className="preference-btn" >Block User</button>
        <button className="preference-btn" >Report</button>
    </div>)
}
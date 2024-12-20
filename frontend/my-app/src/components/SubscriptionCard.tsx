interface SubscriptionCardProps {
    name: string;
    price: number;
    benefitsList: Array<{ id: number; benefitText: string }>;
}
export default function SubscriptionCard(props: SubscriptionCardProps) {
    return(
    <>
        <div className="subscription-card-wrapper">
          <h3>{props.name}</h3>
          <div>
            <ul className="benefits-list">
                {props.benefitsList.map((benefit) => (
                    <li key={benefit.id}>{benefit.benefitText}</li>
                ))}
            </ul>
          </div>
          <h3>Price: {props.price}$/month</h3>
        </div>
    </>);
}
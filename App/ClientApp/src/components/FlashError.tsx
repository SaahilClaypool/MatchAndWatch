import React from "react";


export interface ErrorResonse {
    type: string,
    title: string,
    status: number,
    errors: Map<string, string[]>
}

export function FlashError(props: ErrorResonse) {
    return (
        <div className="error">
            <h3>
                {props.title}
            </h3>
            {
                props.errors.forEach((errors, field) => (
                    <div>
                        <strong>{field}</strong>
                        <ul>
                            {errors.map((error) => <li>{error}</li>)}
                        </ul>
                    </div>
                ))
            }
        </div >
    )
}
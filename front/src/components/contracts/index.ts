export interface ChatMessage {
    content: string;
    author: string;
}

export interface ChatBoxAppState {
    messages: ChatMessage[];
    setMessages: (messages:ChatMessage[]) => void;
}

export interface ChatBoxProps {
    label: string;
    author: string;
}

export interface ChatFieldProps {
    author: string;
}

export interface ContentProps {
    author: string;
}

export interface ChatBoxMessageProps {
    message: ChatMessage;
    id: number;
    reader: string;
}